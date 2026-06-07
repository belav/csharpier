using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal record PrintedNode(CSharpSyntaxNode Node, Doc Doc);

internal static class InvocationExpression
{
    public static Doc Print(InvocationExpressionSyntax node, PrintingContext context)
    {
        return PrintMemberChain(node, context);
    }

    public static Doc PrintMemberChain(ExpressionSyntax node, PrintingContext context)
    {
        var parent = node.Parent;
        var printedNodes = new List<PrintedNode>();

        FlattenAndPrintNodes(node, printedNodes, context);

        var groups = GroupPrintedNodesOnLines(printedNodes);

        var oneLine = SelectManyDocsToArray(groups);

        var shouldMergeFirstTwoGroups = ShouldMergeFirstTwoGroups(groups, parent);

        var forceOneLine =
            (
                groups.Count <= 2
                && (
                    groups.Any(o =>
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
                        groups.Last().Count == 1
                        && groups.Last()[0].Node is PostfixUnaryExpressionSyntax
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
                    groups.Count > 2 && groups[1].Last().Doc is not Group { Contents: IndentDoc },
                    Doc.Concat(groups[1].Select(o => o.Doc).ToArray())
                )
                : Doc.Null,
            PrintIndentedGroup(groups.Skip(shouldMergeFirstTwoGroups ? 2 : 1).ToList())
        );

        return
            oneLine.Skip(1).Any(DocUtilities.ContainsBreak)
            || groups[0]
                .Any(o =>
                    o.Node
                        is ArrayCreationExpressionSyntax
                            or ObjectCreationExpressionSyntax { Initializer: not null }
                )
            || groups[0].First().Node
                is ParenthesizedExpressionSyntax
                {
                    Expression: SwitchExpressionSyntax or QueryExpressionSyntax
                }
            || (
                parent is ExpressionStatementSyntax expressionStatementSyntax
                && expressionStatementSyntax.SemicolonToken.LeadingTrivia.Any(o => o.IsComment())
            )
            || groups.Count == 1
            ? expanded
            : Doc.ConditionalGroup(Doc.Concat(oneLine), expanded);
    }

    private static void FlattenAndPrintNodes(
        ExpressionSyntax expression,
        List<PrintedNode> printedNodes,
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
            FlattenAndPrintNodes(invocationExpressionSyntax.Expression, printedNodes, context);
            printedNodes.Add(
                new PrintedNode(
                    invocationExpressionSyntax,
                    ArgumentList.Print(invocationExpressionSyntax.ArgumentList, context)
                )
            );
        }
        else if (expression is ElementAccessExpressionSyntax elementAccessExpression)
        {
            FlattenAndPrintNodes(elementAccessExpression.Expression, printedNodes, context);
            printedNodes.Add(
                new PrintedNode(
                    elementAccessExpression,
                    Node.Print(elementAccessExpression.ArgumentList, context)
                )
            );
        }
        else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            FlattenAndPrintNodes(memberAccessExpressionSyntax.Expression, printedNodes, context);
            printedNodes.Add(
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
                printedNodes,
                context
            );
            printedNodes.Add(
                new PrintedNode(
                    conditionalAccessExpressionSyntax,
                    Token.Print(conditionalAccessExpressionSyntax.OperatorToken, context)
                )
            );
            FlattenAndPrintNodes(
                conditionalAccessExpressionSyntax.WhenNotNull,
                printedNodes,
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
            FlattenAndPrintNodes(postfixUnaryExpression.Operand, printedNodes, context);
            printedNodes.Add(
                new PrintedNode(
                    postfixUnaryExpression,
                    Token.Print(postfixUnaryExpression.OperatorToken, context)
                )
            );
        }
        else
        {
            printedNodes.Add(new PrintedNode(expression, Node.Print(expression, context)));
        }
    }

    private static List<List<PrintedNode>> GroupPrintedNodesOnLines(List<PrintedNode> printedNodes)
    {
        // We want to group the printed nodes in the following manner
        //
        //   a?.b.c!.d

        // so that we can print it like this if it breaks
        //   a
        //     ?.b
        //     .c!
        //     .d

        var groups = new List<List<PrintedNode>>();

        var currentGroup = new List<PrintedNode> { printedNodes[0] };
        groups.Add(currentGroup);

        for (var index = 1; index < printedNodes.Count; index++)
        {
            if (printedNodes[index].Node is ConditionalAccessExpressionSyntax)
            {
                currentGroup = [];
                groups.Add(currentGroup);
            }
            else if (
                printedNodes[index].Node
                    is MemberAccessExpressionSyntax
                        or MemberBindingExpressionSyntax
                        or IdentifierNameSyntax
                && printedNodes[index + -1].Node is not ConditionalAccessExpressionSyntax
            )
            {
                currentGroup = [];
                groups.Add(currentGroup);
            }

            currentGroup.Add(printedNodes[index]);
        }

        return groups;
    }

    private static Doc[] SelectManyDocsToArray(List<List<PrintedNode>> groups)
    {
        var arrayLength = 0;
        foreach (var group in groups)
        {
            arrayLength += group.Count;
        }

        var outputArray = new Doc[arrayLength];

        var pos = 0;
        foreach (var group in groups)
        {
            foreach (var node in group)
            {
                outputArray[pos] = node.Doc;
                pos++;
            }
        }

        return outputArray;
    }

    private static Doc PrintIndentedGroup(List<List<PrintedNode>> groups)
    {
        if (groups.Count == 0)
        {
            return Doc.Null;
        }

        var result = new DocListBuilder(groups.Count * 2);

        for (int index = 0; index < groups.Count; index++)
        {
            Doc GetPossibleContents()
            {
                if (index >= groups.Count)
                {
                    return Doc.Null;
                }
                var nextGroup = groups[index];
                if (
                    nextGroup[0].Node
                    is not MemberAccessExpressionSyntax
                    {
                        Name: IdentifierNameSyntax { Identifier.Text: "ThenInclude" }
                    }
                )
                {
                    return Doc.Null;
                }

                index++;
                return Doc.Indent(
                    Doc.HardLine,
                    Doc.Group(nextGroup.Select(p => p.Doc).ToArray()),
                    GetPossibleContents()
                );
            }

            var possibleContents = GetPossibleContents();
            if (possibleContents != Doc.Null)
            {
                index--;
                result.Add(possibleContents);
            }
            else
            {
                var group = groups[index];
                result.Add(Doc.HardLine);
                result.Add(Doc.Group(group.Select(p => p.Doc).ToArray()));
            }
        }

        return Doc.Indent(Doc.Group(result.ToArray()));
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
        List<List<PrintedNode>> groups,
        SyntaxNode? parent
    )
    {
        if (groups.Count < 2 || groups[0].Count != 1)
        {
            return false;
        }

        if (
            groups[1]
                .None(o => o.Node is InvocationExpressionSyntax or ElementAccessExpressionSyntax)
        )
        {
            return true;
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

        if (
            groups[1].Count == 1
            || parent
                is SimpleLambdaExpressionSyntax
                    or ArgumentSyntax
                    or BinaryExpressionSyntax
                    or ExpressionStatementSyntax
            || groups[1].Skip(1).First().Node
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
