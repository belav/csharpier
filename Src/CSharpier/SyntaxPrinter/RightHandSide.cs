using System;
using System.Threading;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class RightHandSide
    {
        public static Doc Print(Doc leftDoc, Doc operatorDoc, ExpressionSyntax rightNode)
        {
            var layout = DetermineLayout(rightNode);

            var groupId = Guid.NewGuid().ToString();

            return layout switch
            {
                Layout.BasicConcatWithoutLine
                  => Doc.Concat(leftDoc, operatorDoc, Node.Print(rightNode)),
                Layout.BreakAfterOperator
                  => Doc.Group(
                      Doc.Group(leftDoc),
                      operatorDoc,
                      Doc.Group(Doc.Indent(Doc.Line, Node.Print(rightNode)))
                  ),
                Layout.Fluid
                  => Doc.Group(
                      Doc.Group(leftDoc),
                      operatorDoc,
                      Doc.GroupWithId(groupId, Doc.Indent(Doc.Line)),
                      Doc.IndentIfBreak(Node.Print(rightNode), groupId)
                  ),
                _ => throw new Exception("The layout type of " + layout + " was not handled.")
            };
        }

        private static Layout DetermineLayout(ExpressionSyntax rightNode)
        {
            return rightNode switch
            {
                InitializerExpressionSyntax => Layout.BasicConcatWithoutLine,
                BinaryExpressionSyntax
                or ImplicitObjectCreationExpressionSyntax
                or InterpolatedStringExpressionSyntax
                or IsPatternExpressionSyntax
                or LiteralExpressionSyntax
                or QueryExpressionSyntax
                or StackAllocArrayCreationExpressionSyntax
                or MemberAccessExpressionSyntax
                or ConditionalExpressionSyntax
                {
                    Condition: BinaryExpressionSyntax or ParenthesizedExpressionSyntax
                }
                or CastExpressionSyntax
                {
                    Type: GenericNameSyntax
                }
                  => Layout.BreakAfterOperator,
                _ => Layout.Fluid
            };
        }

        private enum Layout
        {
            BasicConcatWithoutLine,
            BreakAfterOperator,
            Fluid
        }
    }
}
