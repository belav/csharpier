using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class InitializerExpression
    {
        public static Doc Print(InitializerExpressionSyntax node)
        {
            return Print(node, null);
        }

        public static Doc PrintWithConditionalSpace(
            InitializerExpressionSyntax node,
            string groupId
        ) {
            return Print(node, groupId);
        }

        private static Doc Print(InitializerExpressionSyntax node, string? groupId)
        {
            var result = Doc.Concat(
                groupId != null ? Doc.IfBreak(" ", Doc.Line, groupId) : Doc.Null,
                Token.Print(node.OpenBraceToken),
                Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(node.Expressions, Node.Print, Doc.Line)
                ),
                Doc.Line,
                Token.Print(node.CloseBraceToken)
            );
            return node.Parent
                is not (ObjectCreationExpressionSyntax
                    or ArrayCreationExpressionSyntax
                    or ImplicitArrayCreationExpressionSyntax)
            && node.Kind() is not SyntaxKind.WithInitializerExpression ? Doc.Group(result) : result;
        }
    }
}
