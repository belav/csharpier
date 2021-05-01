using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public record PrintedNode(CSharpSyntaxNode Node, Doc Doc);

    public partial class Printer
    {
        private Doc PrintInvocationExpressionSyntax(
            InvocationExpressionSyntax node
        ) {
            var printedNodes = new List<PrintedNode>();

            void Traverse(ExpressionSyntax expression)
            {
                /*InvocationExpression
                  [this.DoSomething().DoSomething][()]
                  
                  SimpleMemberAccessExpression
                  [this.DoSomething()][.][DoSomething]
                  
                  InvocationExpression
                  [this.DoSomething][()]
                  
                  SimpleMemberAccessExpression
                  [this][.][DoSomething]
                */
                if (
                    expression is InvocationExpressionSyntax invocationExpressionSyntax
                ) {
                    Traverse(invocationExpressionSyntax.Expression);
                    printedNodes.Add(
                        new PrintedNode(
                            Node: invocationExpressionSyntax,
                            Doc: this.PrintArgumentListSyntax(
                                invocationExpressionSyntax.ArgumentList
                            )
                        )
                    );
                }
                else if (
                    expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax
                ) {
                    Traverse(memberAccessExpressionSyntax.Expression);
                    printedNodes.Add(
                        new PrintedNode(
                            Node: memberAccessExpressionSyntax,
                            Doc: Doc.Concat(
                                this.PrintSyntaxToken(
                                    memberAccessExpressionSyntax.OperatorToken
                                ),
                                this.Print(memberAccessExpressionSyntax.Name)
                            )
                        )
                    );
                }
                else
                {
                    printedNodes.Add(
                        new PrintedNode(
                            Node: expression,
                            Doc: this.Print(expression)
                        )
                    );
                }
            }

            Traverse(node);

            var groups = new List<List<Doc>>();
            var currentGroup = new List<Doc> { printedNodes[0].Doc };
            var index = 1;
            for (; index < printedNodes.Count; index++)
            {
                if (printedNodes[index].Node is MemberAccessExpressionSyntax)
                {
                    currentGroup.Add(printedNodes[index].Doc);
                }
                else
                {
                    break;
                }
            }

            // TODO GH-7 there is a lot more code in prettier for how to get this all working, also include some documents
            if (printedNodes[index].Node is MemberAccessExpressionSyntax)
            {
                currentGroup.Add(printedNodes[index].Doc);
                index++;
            }

            groups.Add(currentGroup);
            currentGroup = new List<Doc>();

            var hasSeenInvocationExpression = false;
            for (; index < printedNodes.Count; index++)
            {
                if (
                    hasSeenInvocationExpression &&
                    IsMemberish(printedNodes[index].Node)
                ) {
                    // [0] should be appended at the end of the group instead of the
                    // beginning of the next one
                    // if (printedNodes[i].node.computed && isNumericLiteral(printedNodes[i].node.property)) {
                    //     currentGroup.push(printedNodes[i]);
                    //     continue;
                    // }
                    groups.Add(currentGroup);
                    currentGroup = new List<Doc>();
                    hasSeenInvocationExpression = false;
                }

                if (printedNodes[index].Node is InvocationExpressionSyntax)
                {
                    hasSeenInvocationExpression = true;
                }
                currentGroup.Add(printedNodes[index].Doc);
                // if (printedNodes[i].node.comments && printedNodes[i].node.comments.some(comment => comment.trailing)) {
                //     groups.push(currentGroup);
                //     currentGroup = [];
                //     hasSeenCallExpression = false;
                // }
            }

            if (currentGroup.Any())
            {
                groups.Add(currentGroup);
            }

            var cutoff = 3;
            if (groups.Count < cutoff)
            {
                return Doc.Group(groups.SelectMany(o => o).ToArray());
            }

            return Doc.Concat(
                Doc.Group(groups[0].ToArray()),
                PrintIndentedGroup(groups.Skip(1))
            );
        }

        private bool IsMemberish(CSharpSyntaxNode node)
        {
            return node is MemberAccessExpressionSyntax;
        }

        private Doc PrintIndentedGroup(IEnumerable<List<Doc>> groups)
        {
            if (!groups.Any())
            {
                return Doc.Null;
            }

            // TODO GH-7 softline here?
            return Doc.Indent(
                Doc.Group(
                    Doc.Join(
                        Doc.SoftLine,
                        groups.Select(o => Doc.Group(o.ToArray()))
                    )
                )
            );
        }
    }
}
