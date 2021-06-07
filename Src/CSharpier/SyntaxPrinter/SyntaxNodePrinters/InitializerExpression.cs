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
            var result = Doc.Concat(
                node.Kind()
                    is (SyntaxKind.ArrayInitializerExpression
                        or SyntaxKind.ComplexElementInitializerExpression) ? Doc.Null : Doc.Line,
                Token.Print(node.OpenBraceToken),
                Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(node.Expressions, Node.Print, Doc.Line)
                ),
                Doc.Line,
                Token.Print(node.CloseBraceToken)
            );
            return node.Kind() is SyntaxKind.ComplexElementInitializerExpression
                ? Doc.Group(result)
                : result;
        }
    }
}
