using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class RightHandSide
    {
        public static Doc Print(ExpressionSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();
            return node switch
            {
                InitializerExpressionSyntax initializerExpressionSyntax
                  => InitializerExpression.Print(initializerExpressionSyntax),
                InvocationExpressionSyntax
                or ParenthesizedLambdaExpressionSyntax
                or ObjectCreationExpressionSyntax
                or ElementAccessExpressionSyntax
                or ArrayCreationExpressionSyntax
                or ImplicitArrayCreationExpressionSyntax
                  => Doc.Group(
                      Doc.GroupWithId(groupId, Doc.Indent(Doc.Line)),
                      Doc.IndentIfBreak(Doc.Group(Node.Print(node)), groupId)
                  ),
                AnonymousObjectCreationExpressionSyntax
                or AnonymousMethodExpressionSyntax
                or ConditionalExpressionSyntax
                or SwitchExpressionSyntax
                or LambdaExpressionSyntax
                or AwaitExpressionSyntax
                or WithExpressionSyntax
                  => Doc.Group(Doc.Group(Doc.Indent(Doc.Line)), Node.Print(node)),
                BinaryExpressionSyntax
                or InterpolatedStringExpressionSyntax
                or IsPatternExpressionSyntax
                or LiteralExpressionSyntax
                or QueryExpressionSyntax
                or StackAllocArrayCreationExpressionSyntax
                  => Doc.Indent(Doc.Group(Doc.Line, Node.Print(node))),
                _ => Doc.Group(Doc.Indent(Doc.Line), Doc.Indent(Node.Print(node)))
            };
        }
    }
}
