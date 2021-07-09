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

        public static Doc PrintWithLine(InitializerExpressionSyntax node)
        {
            return Print(node, null, true);
        }

        private static Doc Print(
            InitializerExpressionSyntax node,
            string? groupId,
            bool useLine = false
        ) {
            var result = Doc.Concat(
                groupId != null
                    ? Doc.IfBreak(" ", Doc.Line, groupId)
                    : useLine ? Doc.Line : Doc.Null,
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
