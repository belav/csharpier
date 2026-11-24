using System.Management.Automation.Language;

namespace CSharpier.Core.PowerShell;

internal sealed class AttachedComments
{
    internal Dictionary<Ast, List<IScriptExtent>> Leading { get; } = [];
    internal Dictionary<Ast, List<IScriptExtent>> Trailing { get; } = [];
    internal Dictionary<Ast, List<IScriptExtent>> Dangling { get; } = [];
}

// The PowerShell AST has no trivia, so each comment token is assigned to an AST node before
// printing. A comment on the same line as the end of the node before it is a trailing comment of
// that node, otherwise it is a leading comment of the node after it, otherwise it is a dangling
// comment of the node that contains it.
internal static class CommentAttacher
{
    internal static AttachedComments Attach(
        ScriptBlockAst root,
        IReadOnlyList<IScriptExtent> comments
    )
    {
        var result = new AttachedComments();
        if (comments.Count == 0)
        {
            return result;
        }

        var children = GetChildren(root);
        var statementsByBlock = new Dictionary<NamedBlockAst, List<StatementAst>>();

        foreach (var comment in comments)
        {
            Ast enclosing = root;
            Ast? preceding = null;
            Ast? following = null;

            while (children.TryGetValue(enclosing, out var enclosingChildren))
            {
                var container = enclosingChildren.FindContainer(comment);
                if (container is null)
                {
                    preceding = enclosingChildren.FindPreceding(comment);
                    following = enclosingChildren.FindFollowing(comment);
                    break;
                }

                enclosing = container;
            }

            if (
                preceding is not null
                && preceding.Extent.EndLineNumber == comment.StartLineNumber
                && TryAdd(
                    result.Trailing,
                    GetLastStatementIfUnnamedBlock(preceding, statementsByBlock),
                    comment
                )
            )
            {
                continue;
            }

            if (
                following is not null
                && TryAdd(
                    result.Leading,
                    GetFirstStatementIfUnnamedBlock(following, statementsByBlock),
                    comment
                )
            )
            {
                continue;
            }

            while (enclosing is NamedBlockAst { Unnamed: true } && enclosing.Parent is not null)
            {
                enclosing = enclosing.Parent;
            }

            // The statement block of @( ) and $( ) does not cover the delimiters, so comments
            // next to them land on the expression. The block is what prints them.
            enclosing = enclosing switch
            {
                ArrayExpressionAst arrayExpression => arrayExpression.SubExpression,
                SubExpressionAst subExpression => subExpression.SubExpression,
                _ => enclosing,
            };

            TryAdd(result.Dangling, enclosing, comment);
        }

        return result;
    }

    // The unnamed end block of a script is never printed as a node of its own, so comments around
    // it belong to its first or last statement.
    private static Ast? GetFirstStatementIfUnnamedBlock(
        Ast node,
        Dictionary<NamedBlockAst, List<StatementAst>> statementsByBlock
    )
    {
        if (node is NamedBlockAst { Unnamed: true } block)
        {
            var statements = GetStatementsAndTraps(block, statementsByBlock);
            return statements.Count > 0 ? statements[0] : null;
        }

        if (node is StatementBlockAst { Parent: ArrayExpressionAst or SubExpressionAst } inner)
        {
            return inner.Statements.Count > 0 ? inner.Statements[0] : null;
        }

        return node;
    }

    private static Ast? GetLastStatementIfUnnamedBlock(
        Ast node,
        Dictionary<NamedBlockAst, List<StatementAst>> statementsByBlock
    )
    {
        if (node is NamedBlockAst { Unnamed: true } block)
        {
            var statements = GetStatementsAndTraps(block, statementsByBlock);
            return statements.Count > 0 ? statements[^1] : null;
        }

        if (node is StatementBlockAst { Parent: ArrayExpressionAst or SubExpressionAst } inner)
        {
            return inner.Statements.Count > 0 ? inner.Statements[^1] : null;
        }

        return node;
    }

    // Many comments can sit before the first statement of a block, so the sorted statements are
    // kept for each block.
    private static List<StatementAst> GetStatementsAndTraps(
        NamedBlockAst block,
        Dictionary<NamedBlockAst, List<StatementAst>> statementsByBlock
    )
    {
        if (!statementsByBlock.TryGetValue(block, out var statements))
        {
            statements = block
                .Statements.Concat(block.Traps ?? [])
                .Where(o => !Statements.IsBaseConstructorCall(o))
                .OrderBy(o => o.Extent.StartOffset)
                .ToList();
            statementsByBlock[block] = statements;
        }

        return statements;
    }

    private static bool TryAdd(
        Dictionary<Ast, List<IScriptExtent>> comments,
        Ast? node,
        IScriptExtent comment
    )
    {
        if (node is null)
        {
            return false;
        }

        if (!comments.TryGetValue(node, out var list))
        {
            list = [];
            comments[node] = list;
        }

        list.Add(comment);
        return true;
    }

    private static Dictionary<Ast, SortedChildren> GetChildren(ScriptBlockAst root)
    {
        var lists = new Dictionary<Ast, List<Ast>>();
        foreach (var node in root.FindAll(_ => true, searchNestedScriptBlocks: true))
        {
            if (node.Parent is null || node.Extent.StartOffset == node.Extent.EndOffset)
            {
                continue;
            }

            // FindAll skips the call to a base constructor, and the tree has it in the body of the
            // constructor although it is written in front of the body, so it becomes a child of
            // the function definition.
            if (
                node is CommandExpressionAst { Expression: BaseCtorInvokeMemberExpressionAst call }
                && node.Parent.Parent?.Parent is FunctionDefinitionAst function
                && call.Extent.EndOffset <= function.Body.Extent.StartOffset
            )
            {
                AddChild(lists, function, call);
                continue;
            }

            AddChild(lists, node.Parent, node);
        }

        var children = new Dictionary<Ast, SortedChildren>(lists.Count);
        foreach (var (parent, list) in lists)
        {
            list.Sort(
                (first, second) =>
                    PrintContext
                        .GetOwnStartOffset(first)
                        .CompareTo(PrintContext.GetOwnStartOffset(second))
            );
            children[parent] = new SortedChildren(list);
        }

        return children;
    }

    private static void AddChild(Dictionary<Ast, List<Ast>> lists, Ast parent, Ast child)
    {
        if (!lists.TryGetValue(parent, out var list))
        {
            list = [];
            lists[parent] = list;
        }

        list.Add(child);
    }

    // The children of one node, sorted by where they start, with binary searches that give the
    // same answers as checking every child in that order. Siblings can overlap (the unnamed end
    // block of a script starts where its param block starts), so the end offsets are not assumed
    // to be in the same order, and the searches by end offset use their own arrays.
    private sealed class SortedChildren
    {
        private readonly List<Ast> byStart;
        private readonly int[] starts;

        // The greatest end offset among byStart[0..index].
        private readonly int[] greatestEnds;

        // Sorted by end offset, and by position in byStart when two children end at the same
        // offset.
        private readonly Ast[] byEnd;
        private readonly int[] ends;

        internal SortedChildren(List<Ast> byStart)
        {
            this.byStart = byStart;
            this.starts = new int[byStart.Count];
            this.greatestEnds = new int[byStart.Count];
            var order = new int[byStart.Count];
            for (var index = 0; index < byStart.Count; index++)
            {
                this.starts[index] = PrintContext.GetOwnStartOffset(byStart[index]);
                this.greatestEnds[index] = Math.Max(
                    index > 0 ? this.greatestEnds[index - 1] : int.MinValue,
                    byStart[index].Extent.EndOffset
                );
                order[index] = index;
            }

            Array.Sort(order, (first, second) =>
            {
                var result = byStart[first]
                    .Extent.EndOffset.CompareTo(byStart[second].Extent.EndOffset);
                return result != 0 ? result : first.CompareTo(second);
            });

            this.byEnd = new Ast[byStart.Count];
            this.ends = new int[byStart.Count];
            for (var index = 0; index < order.Length; index++)
            {
                this.byEnd[index] = byStart[order[index]];
                this.ends[index] = this.byEnd[index].Extent.EndOffset;
            }
        }

        // The first child that covers the comment.
        internal Ast? FindContainer(IScriptExtent comment)
        {
            var index = FindFirstAtLeast(this.greatestEnds, comment.EndOffset);
            return index < this.starts.Length && this.starts[index] <= comment.StartOffset
                ? this.byStart[index]
                : null;
        }

        // The child that ends last at or before the start of the comment, and of those that end
        // there, the last one in byStart.
        internal Ast? FindPreceding(IScriptExtent comment)
        {
            var index = FindFirstAtLeast(this.ends, comment.StartOffset + 1) - 1;
            return index >= 0 ? this.byEnd[index] : null;
        }

        // The first child that starts at or after the end of the comment.
        internal Ast? FindFollowing(IScriptExtent comment)
        {
            var index = FindFirstAtLeast(this.starts, comment.EndOffset);
            return index < this.starts.Length ? this.byStart[index] : null;
        }

        private static int FindFirstAtLeast(int[] sortedValues, int value)
        {
            var low = 0;
            var high = sortedValues.Length;
            while (low < high)
            {
                var middle = (low + high) / 2;
                if (sortedValues[middle] < value)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle;
                }
            }

            return low;
        }
    }
}
