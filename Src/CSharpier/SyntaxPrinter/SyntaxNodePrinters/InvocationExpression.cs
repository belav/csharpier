using System.Diagnostics.CodeAnalysis;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal record struct PrintedNode(CSharpSyntaxNode Node, Doc Doc);

// This is based on prettier/src/language-js/print/member-chain.js
// various discussions/prs about how to potentially improve the formatting
// https://github.com/prettier/prettier/issues/5737
// https://github.com/prettier/prettier/issues/7884
// https://github.com/prettier/prettier/issues/8003
// https://github.com/prettier/prettier/issues/8902
// https://github.com/prettier/prettier/pull/7889
internal static class InvocationExpression
{
    public static Doc Print(InvocationExpressionSyntax node, PrintingContext context)
    {
        return PrintMemberChain(node, context);
    }

    public static Doc PrintMemberChain(ExpressionSyntax node, PrintingContext context)
    {
        var parent = node.Parent;
        var printedNodes = new ValueListBuilder<PrintedNode>(
            [default, default, default, default, default, default, default, default]
        );

        FlattenAndPrintNodes(node, ref printedNodes, context);

        var anyInvocationExpression = false;

        foreach (var printedNode in printedNodes.AsSpan())
        {
            if (printedNode.Node is InvocationExpressionSyntax)
            {
                anyInvocationExpression = true;
                break;
            }
        }

        var groups = new ValueListBuilder<PrintedNode[]>(
            [null, null, null, null, null, null, null, null]
        );

        if (anyInvocationExpression)
        {
            GroupPrintedNodesPrettierStyle(ref groups, ref printedNodes);
        }
        else
        {
            GroupPrintedNodesOnLines(ref groups, ref printedNodes);
        }

        var oneLine = SelectManyDocsToArray(ref groups);

        var shouldMergeFirstTwoGroups = ShouldMergeFirstTwoGroups(ref groups, parent);

        var cutoff = shouldMergeFirstTwoGroups ? 3 : 2;

        var forceOneLine =
            (
                groups.Length <= cutoff
                && (
                    groups
                        .AsSpan()
                        .Skip(shouldMergeFirstTwoGroups ? 1 : 0)
                        .Any(o =>
                            o.Last().Node
                                is not (
                                    InvocationExpressionSyntax
                                    or ElementAccessExpressionSyntax
                                    or PostfixUnaryExpressionSyntax
                                    {
                                        Operand: InvocationExpressionSyntax
                                    }
                                )
                        )
                    // if the last group contains just a !, make sure it doesn't end up on a new line
                    || (
                        groups[^1].Length == 1 && groups[^1][0].Node is PostfixUnaryExpressionSyntax
                    )
                )
            )
            || node.HasParent(typeof(InterpolatedStringExpressionSyntax))
            // this handles the case of a multiline string being part of an invocation chain
            // conditional groups don't propagate breaks so we need to avoid the conditional group
            || groups[0]
                .Any(o =>
                    o.Node
                        is LiteralExpressionSyntax
                            {
                                Token.RawKind: (int)SyntaxKind.MultiLineRawStringLiteralToken
                            }
                            or InterpolatedStringExpressionSyntax
                            {
                                StringStartToken.RawKind: (int)
                                    SyntaxKind.InterpolatedMultiLineRawStringStartToken
                            }
                    || o.Node
                        is LiteralExpressionSyntax
                        {
                            Token.Text.Length: > 0
                        } literalExpressionSyntax
                        && literalExpressionSyntax.Token.Text.Contains('\n')
                );

        if (forceOneLine)
        {
            return Doc.Group(oneLine);
        }

        var expanded = Doc.Concat(
            Doc.Concat(groups[0].Select(o => o.Doc).ToArray()),
            shouldMergeFirstTwoGroups
                ? Doc.IndentIf(
                    groups.Length > 2 && groups[1].Last().Doc is not Group { Contents: IndentDoc },
                    Doc.Concat(groups[1].Select(o => o.Doc).ToArray())
                )
                : Doc.Null,
            PrintIndentedGroup(groups.AsSpan()[(shouldMergeFirstTwoGroups ? 2 : 1)..])
        );

        return
            oneLine.AsSpan().Skip(1).Any(DocUtilities.ContainsBreak)
            || groups[0]
                .Any(o =>
                    o.Node
                        is ArrayCreationExpressionSyntax
                            or ObjectCreationExpressionSyntax { Initializer: not null }
                )
            || groups[0].First().Node
                is ParenthesizedExpressionSyntax { Expression: SwitchExpressionSyntax }
            ? expanded
            : Doc.ConditionalGroup(Doc.Concat(oneLine), expanded);
    }

    private static void FlattenAndPrintNodes(
        ExpressionSyntax expression,
        ref ValueListBuilder<PrintedNode> printedNodes,
        PrintingContext context
    )
    {
        /*
          We need to flatten things out because the AST has them this way
          InvocationExpression
          Expression                        ArgumentList
          this.DoSomething().DoSomething    ()
          
          MemberAccessExpression
          Expression            OperatorToken   Name
          this.DoSomething()    .               DoSomething
          
          InvocationExpression
          Expression            ArgumentList
          this.DoSomething      ()
          
          MemberAccessExpression
          Expression    OperatorToken   Name
          this          .               DoSomething
          
          And we want to work with them from Left to Right
        */
        if (expression is InvocationExpressionSyntax invocationExpressionSyntax)
        {
            FlattenAndPrintNodes(invocationExpressionSyntax.Expression, ref printedNodes, context);
            printedNodes.Append(
                new PrintedNode(
                    invocationExpressionSyntax,
                    ArgumentList.Print(invocationExpressionSyntax.ArgumentList, context)
                )
            );
        }
        else if (expression is ElementAccessExpressionSyntax elementAccessExpression)
        {
            FlattenAndPrintNodes(elementAccessExpression.Expression, ref printedNodes, context);
            printedNodes.Append(
                new PrintedNode(
                    elementAccessExpression,
                    Node.Print(elementAccessExpression.ArgumentList, context)
                )
            );
        }
        else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            FlattenAndPrintNodes(
                memberAccessExpressionSyntax.Expression,
                ref printedNodes,
                context
            );
            printedNodes.Append(
                new PrintedNode(
                    memberAccessExpressionSyntax,
                    Doc.Concat(
                        Token.Print(memberAccessExpressionSyntax.OperatorToken, context),
                        Node.Print(memberAccessExpressionSyntax.Name, context)
                    )
                )
            );
        }
        else if (expression is ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax)
        {
            FlattenAndPrintNodes(
                conditionalAccessExpressionSyntax.Expression,
                ref printedNodes,
                context
            );
            printedNodes.Append(
                new PrintedNode(
                    conditionalAccessExpressionSyntax,
                    Token.Print(conditionalAccessExpressionSyntax.OperatorToken, context)
                )
            );
            FlattenAndPrintNodes(
                conditionalAccessExpressionSyntax.WhenNotNull,
                ref printedNodes,
                context
            );
        }
        else if (
            expression is PostfixUnaryExpressionSyntax
            {
                Operand: InvocationExpressionSyntax or MemberAccessExpressionSyntax
            } postfixUnaryExpression
        )
        {
            FlattenAndPrintNodes(postfixUnaryExpression.Operand, ref printedNodes, context);
            printedNodes.Append(
                new PrintedNode(
                    postfixUnaryExpression,
                    Token.Print(postfixUnaryExpression.OperatorToken, context)
                )
            );
        }
        else
        {
            printedNodes.Append(new PrintedNode(expression, Node.Print(expression, context)));
        }
    }

    private static void GroupPrintedNodesOnLines(
        ref ValueListBuilder<PrintedNode[]> groups,
        ref ValueListBuilder<PrintedNode> printedNodes
    )
    {
        // We want to group the printed nodes in the following manner
        //
        //   a?.b.c!.d

        // so that we can print it like this if it breaks
        //   a
        //     ?.b
        //     .c!
        //     .d
        var currentGroup = new ValueListBuilder<PrintedNode>([default, default, default, default]);
        currentGroup.Append(printedNodes[0]);

        for (var index = 1; index < printedNodes.Length; index++)
        {
            if (printedNodes[index].Node is ConditionalAccessExpressionSyntax)
            {
                groups.Append(currentGroup.AsSpan().ToArray());
                currentGroup.Length = 0;
            }
            else if (
                printedNodes[index].Node
                    is MemberAccessExpressionSyntax
                        or MemberBindingExpressionSyntax
                        or IdentifierNameSyntax
                && printedNodes[index + -1].Node is not ConditionalAccessExpressionSyntax
            )
            {
                groups.Append(currentGroup.AsSpan().ToArray());
                currentGroup.Length = 0;
            }

            currentGroup.Append(printedNodes[index]);
        }

        if (currentGroup.Length > 0)
        {
            groups.Append(currentGroup.AsSpan().ToArray());
        }
    }

    private static void GroupPrintedNodesPrettierStyle(
        ref ValueListBuilder<PrintedNode[]> groups,
        ref ValueListBuilder<PrintedNode> printedNodes
    )
    {
        // We want to group the printed nodes in the following manner
        //
        //   a().b.c().d().e
        // will be grouped as
        //   [
        //     [Identifier, InvocationExpression],
        //     [MemberAccessExpression], [MemberAccessExpression, InvocationExpression],
        //     [MemberAccessExpression, InvocationExpression],
        //     [MemberAccessExpression],
        //   ]

        // so that we can print it as
        //   a()
        //     .b.c()
        //     .d()
        //     .e

        // TODO #451 this whole thing could possibly just turn into a big loop
        // based on the current node, and the next/previous node, decide when to create new groups.
        // certain nodes need to stay in the current group, other nodes indicate that a new group needs to be created.
        var currentGroup = new ValueListBuilder<PrintedNode>([default, default, default, default]);
        currentGroup.Append(printedNodes[0]);

        var index = 1;
        for (; index < printedNodes.Length; index++)
        {
            if (printedNodes[index].Node is InvocationExpressionSyntax)
            {
                currentGroup.Append(printedNodes[index]);
            }
            else
            {
                break;
            }
        }

        if (
            printedNodes[0].Node is not (InvocationExpressionSyntax or PostfixUnaryExpressionSyntax)
            && index < printedNodes.Length
            && printedNodes[index].Node
                is ElementAccessExpressionSyntax
                    or PostfixUnaryExpressionSyntax
        )
        {
            currentGroup.Append(printedNodes[index]);
            index++;
        }

        groups.Append(currentGroup.AsSpan().ToArray());
        currentGroup.Length = 0;

        var hasSeenNodeThatRequiresBreak = false;
        for (; index < printedNodes.Length; index++)
        {
            if (
                hasSeenNodeThatRequiresBreak
                && printedNodes[index].Node
                    is MemberAccessExpressionSyntax
                        or ConditionalAccessExpressionSyntax
            )
            {
                groups.Append(currentGroup.AsSpan().ToArray());
                currentGroup.Length = 0;
                hasSeenNodeThatRequiresBreak = false;
            }

            if (
                printedNodes[index].Node
                is (InvocationExpressionSyntax or ElementAccessExpressionSyntax)
            )
            {
                hasSeenNodeThatRequiresBreak = true;
            }
            currentGroup.Append(printedNodes[index]);
        }

        if (currentGroup.Length != 0)
        {
            groups.Append(currentGroup.AsSpan().ToArray());
        }
    }

    [SuppressMessage("ReSharper", "ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator")]
    private static Doc[] SelectManyDocsToArray(ref ValueListBuilder<PrintedNode[]> groups)
    {
        var arrayLength = 0;
        foreach (var group in groups.AsSpan())
        {
            arrayLength += group.Length;
        }

        var outputArray = new Doc[arrayLength];

        var pos = 0;
        foreach (var group in groups.AsSpan())
        {
            foreach (var node in group)
            {
                outputArray[pos] = node.Doc;
                pos++;
            }
        }

        return outputArray;
    }

    private static Doc PrintIndentedGroup(ReadOnlySpan<PrintedNode[]> groups)
    {
        if (groups.Length == 0)
        {
            return Doc.Null;
        }

        return Doc.Indent(
            Doc.Group(
                Doc.HardLine,
                Doc.Join(
                    Doc.HardLine,
                    groups.ToArray().Select(o => Doc.Group(o.Select(p => p.Doc).ToArray()))
                )
            )
        );
    }

    // There are cases where merging the first two groups looks better
    // For example
    /*
        // without merging we get this
        this
            .CallMethod()
            .CallMethod();

        // merging gives us this
        this.CallMethod()
            .CallMethod();
     */
    private static bool ShouldMergeFirstTwoGroups(
        ref ValueListBuilder<PrintedNode[]> groups,
        SyntaxNode? parent
    )
    {
        if (groups.Length < 2 || groups[0].Length != 1)
        {
            return false;
        }

        var firstNode = groups[0][0].Node;

        if (
            firstNode
            is not (
                IdentifierNameSyntax { Identifier.Text.Length: <= 4 }
                or ThisExpressionSyntax
                or PredefinedTypeSyntax
                or BaseExpressionSyntax
            )
        )
        {
            return false;
        }

        // TODO maybe some things to fix in here
        // https://github.com/belav/csharpier-repos/pull/100/files
        if (
            groups[1].Length == 1
            || parent
                is SimpleLambdaExpressionSyntax
                    or ArgumentSyntax
                    or BinaryExpressionSyntax
                    or ExpressionStatementSyntax
            || groups[1][1].Node
                is InvocationExpressionSyntax
                    or ElementAccessExpressionSyntax
                    or PostfixUnaryExpressionSyntax
        )
        {
            return true;
        }

        return false;
    }
}
