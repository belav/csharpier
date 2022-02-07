namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal record PrintedNode(CSharpSyntaxNode Node, Doc Doc);

// This is based on prettier/src/language-js/print/member-chain.js
// various discussions/prs about how to potentially improve the formatting
// https://github.com/prettier/prettier/issues/5737
// https://github.com/prettier/prettier/issues/7884
// https://github.com/prettier/prettier/issues/8003
// https://github.com/prettier/prettier/issues/8902
// https://github.com/prettier/prettier/pull/7889
internal static class InvocationExpression
{
    public static Doc Print(InvocationExpressionSyntax node)
    {
        return PrintMemberChain(node);
    }

    public static Doc PrintMemberChain(ExpressionSyntax node)
    {
        var printedNodes = new List<PrintedNode>();

        FlattenAndPrintNodes(node, printedNodes);

        var groups = GroupPrintedNodes(printedNodes);

        var oneLine = groups.SelectMany(o => o).Select(o => o.Doc).ToArray();

        var shouldMergeFirstTwoGroups = ShouldMergeFirstTwoGroups(groups);

        var cutoff = shouldMergeFirstTwoGroups ? 3 : 2;

        var forceOneLine =
            groups.Count <= cutoff
            && !groups
                .Skip(shouldMergeFirstTwoGroups ? 1 : 0)
                .All(o => o.Last().Node is InvocationExpressionSyntax);

        if (forceOneLine)
        {
            return Doc.Group(oneLine);
        }

        var expanded = Doc.Concat(
            Doc.Concat(groups[0].Select(o => o.Doc).ToArray()),
            shouldMergeFirstTwoGroups
              ? Doc.Concat(groups[1].Select(o => o.Doc).ToArray())
              : Doc.Null,
            PrintIndentedGroup(node, groups.Skip(shouldMergeFirstTwoGroups ? 2 : 1).ToList())
        );

        return oneLine.Skip(1).Any(DocUtilities.ContainsBreak)
          ? expanded
          : Doc.ConditionalGroup(Doc.Concat(oneLine), expanded);
    }

    private static void FlattenAndPrintNodes(
        ExpressionSyntax expression,
        List<PrintedNode> printedNodes
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
            FlattenAndPrintNodes(invocationExpressionSyntax.Expression, printedNodes);
            printedNodes.Add(
                new PrintedNode(
                    invocationExpressionSyntax,
                    ArgumentList.Print(invocationExpressionSyntax.ArgumentList)
                )
            );
        }
        else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            FlattenAndPrintNodes(memberAccessExpressionSyntax.Expression, printedNodes);
            printedNodes.Add(
                new PrintedNode(
                    memberAccessExpressionSyntax,
                    Doc.Concat(
                        Token.Print(memberAccessExpressionSyntax.OperatorToken),
                        Node.Print(memberAccessExpressionSyntax.Name)
                    )
                )
            );
        }
        else if (expression is ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax)
        {
            FlattenAndPrintNodes(conditionalAccessExpressionSyntax.Expression, printedNodes);
            printedNodes.Add(
                new PrintedNode(
                    conditionalAccessExpressionSyntax,
                    Token.Print(conditionalAccessExpressionSyntax.OperatorToken)
                )
            );
            FlattenAndPrintNodes(conditionalAccessExpressionSyntax.WhenNotNull, printedNodes);
        }
        else if (expression is PostfixUnaryExpressionSyntax postfixUnaryExpression)
        {
            FlattenAndPrintNodes(postfixUnaryExpression.Operand, printedNodes);
            printedNodes.Add(
                new PrintedNode(
                    postfixUnaryExpression,
                    Token.Print(postfixUnaryExpression.OperatorToken)
                )
            );
        }
        else
        {
            printedNodes.Add(new PrintedNode(expression, Node.Print(expression)));
        }
    }

    private static List<List<PrintedNode>> GroupPrintedNodes(List<PrintedNode> printedNodes)
    {
        // We want to group the printed nodes in the following manner
        //
        //   a().b.c().d().e
        // will be grouped as
        //   [
        //     [Identifier, InvocationExpression],
        //     [MemberAccessExpression, MemberAccessExpression, InvocationExpression],
        //     [MemberAccessExpression, InvocationExpression],
        //     [MemberAccessExpression],
        //   ]

        // so that we can print it as
        //   a()
        //     .b.c()
        //     .d()
        //     .e

        // The first group is the first node followed by
        //   - as many InvocationExpression as possible
        //       < fn()()() >.something()
        //   - as many array accessors as possible
        //       < fn()[0][1][2] >.something()
        //   - then, as many MemberAccessExpression as possible but the last one
        //       < this.items >.something()

        var groups = new List<List<PrintedNode>>();
        var currentGroup = new List<PrintedNode> { printedNodes[0] };
        var index = 1;
        for (; index < printedNodes.Count; index++)
        {
            if (printedNodes[index].Node is InvocationExpressionSyntax)
            {
                currentGroup.Add(printedNodes[index]);
            }
            else
            {
                break;
            }
        }

        if (printedNodes[0].Node is not InvocationExpressionSyntax)
        {
            for (; index + 1 < printedNodes.Count; ++index)
            {
                if (
                    IsMemberish(printedNodes[index].Node)
                    && IsMemberish(printedNodes[index + 1].Node)
                )
                {
                    currentGroup.Add(printedNodes[index]);
                }
                else
                {
                    break;
                }
            }
        }

        groups.Add(currentGroup);
        currentGroup = new List<PrintedNode>();

        var hasSeenInvocationExpression = false;
        for (; index < printedNodes.Count; index++)
        {
            if (hasSeenInvocationExpression && IsMemberish(printedNodes[index].Node))
            {
                groups.Add(currentGroup);
                currentGroup = new List<PrintedNode>();
                hasSeenInvocationExpression = false;
            }

            if (printedNodes[index].Node is InvocationExpressionSyntax)
            {
                hasSeenInvocationExpression = true;
            }
            currentGroup.Add(printedNodes[index]);
        }

        if (currentGroup.Any())
        {
            groups.Add(currentGroup);
        }

        return groups;
    }

    private static bool IsMemberish(CSharpSyntaxNode node)
    {
        return node
            is MemberAccessExpressionSyntax
                or ConditionalAccessExpressionSyntax
                or PostfixUnaryExpressionSyntax;
    }

    private static Doc PrintIndentedGroup(ExpressionSyntax node, IList<List<PrintedNode>> groups)
    {
        if (groups.Count == 0)
        {
            return Doc.Null;
        }

        var shouldIndent = !(
            node.Parent is ConditionalExpressionSyntax conditionalExpressionSyntax
            && conditionalExpressionSyntax.Condition != node
        );

        return Doc.IndentIf(
            shouldIndent,
            Doc.Group(
                Doc.HardLine,
                Doc.Join(
                    Doc.HardLine,
                    groups.Select(o => Doc.Group(o.Select(p => p.Doc).ToArray()))
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
    private static bool ShouldMergeFirstTwoGroups(List<List<PrintedNode>> groups)
    {
        if (groups.Count < 2 || groups[0].Count != 1)
        {
            return false;
        }

        var firstNode = groups[0][0].Node;

        if (
            firstNode is IdentifierNameSyntax identifierNameSyntax
            && identifierNameSyntax.Identifier.Text.Length <= 4
        )
        {
            return true;
        }

        return firstNode is ThisExpressionSyntax or PredefinedTypeSyntax or BaseExpressionSyntax;
    }
}
