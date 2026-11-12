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

        foreach (var comment in comments)
        {
            Ast enclosing = root;
            Ast? preceding = null;
            Ast? following = null;

            while (true)
            {
                preceding = null;
                following = null;
                Ast? container = null;

                if (children.TryGetValue(enclosing, out var enclosingChildren))
                {
                    foreach (var child in enclosingChildren)
                    {
                        var childStartOffset = PrintContext.GetOwnStartOffset(child);
                        if (
                            childStartOffset <= comment.StartOffset
                            && comment.EndOffset <= child.Extent.EndOffset
                        )
                        {
                            container = child;
                            break;
                        }

                        if (child.Extent.EndOffset <= comment.StartOffset)
                        {
                            if (
                                preceding is null
                                || child.Extent.EndOffset >= preceding.Extent.EndOffset
                            )
                            {
                                preceding = child;
                            }
                        }
                        else if (childStartOffset >= comment.EndOffset)
                        {
                            following ??= child;
                        }
                    }
                }

                if (container is null)
                {
                    break;
                }

                enclosing = container;
            }

            if (
                preceding is not null
                && preceding.Extent.EndLineNumber == comment.StartLineNumber
                && TryAdd(result.Trailing, GetLastStatementIfUnnamedBlock(preceding), comment)
            )
            {
                continue;
            }

            if (
                following is not null
                && TryAdd(result.Leading, GetFirstStatementIfUnnamedBlock(following), comment)
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
    private static Ast? GetFirstStatementIfUnnamedBlock(Ast node)
    {
        if (node is NamedBlockAst { Unnamed: true } block)
        {
            return GetStatementsAndTraps(block.Statements, block.Traps).FirstOrDefault();
        }

        if (node is StatementBlockAst { Parent: ArrayExpressionAst or SubExpressionAst } inner)
        {
            return inner.Statements.Count > 0 ? inner.Statements[0] : null;
        }

        return node;
    }

    private static Ast? GetLastStatementIfUnnamedBlock(Ast node)
    {
        if (node is NamedBlockAst { Unnamed: true } block)
        {
            return GetStatementsAndTraps(block.Statements, block.Traps).LastOrDefault();
        }

        if (node is StatementBlockAst { Parent: ArrayExpressionAst or SubExpressionAst } inner)
        {
            return inner.Statements.Count > 0 ? inner.Statements[^1] : null;
        }

        return node;
    }

    private static IEnumerable<StatementAst> GetStatementsAndTraps(
        IEnumerable<StatementAst> statements,
        IEnumerable<TrapStatementAst>? traps
    )
    {
        return statements
            .Concat(traps ?? [])
            .Where(o => !Statements.IsBaseConstructorCall(o))
            .OrderBy(o => o.Extent.StartOffset);
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

    private static Dictionary<Ast, List<Ast>> GetChildren(ScriptBlockAst root)
    {
        var children = new Dictionary<Ast, List<Ast>>();
        foreach (var node in root.FindAll(_ => true, searchNestedScriptBlocks: true))
        {
            if (node.Parent is null || node.Extent.StartOffset == node.Extent.EndOffset)
            {
                continue;
            }

            if (!children.TryGetValue(node.Parent, out var list))
            {
                list = [];
                children[node.Parent] = list;
            }

            list.Add(node);
        }

        foreach (var list in children.Values)
        {
            list.Sort(
                (first, second) =>
                    PrintContext
                        .GetOwnStartOffset(first)
                        .CompareTo(PrintContext.GetOwnStartOffset(second))
            );
        }

        return children;
    }
}
