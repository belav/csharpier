using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInitializerExpressionSyntax(
            InitializerExpressionSyntax node
        ) {
            var result = Docs.Concat(
                node.Kind() == SyntaxKind.ArrayInitializerExpression
                    ? string.Empty
                    : node.Kind() ==
                            SyntaxKind.ComplexElementInitializerExpression
                            ? Docs.SoftLine
                            : Docs.Line,
                SyntaxTokens.Print(node.OpenBraceToken),
                Docs.Indent(
                    Docs.Line,
                    SeparatedSyntaxList.Print(
                        node.Expressions,
                        this.Print,
                        Docs.Line
                    )
                ),
                Docs.Line,
                SyntaxTokens.Print(node.CloseBraceToken)
            );
            return node.Parent is not ObjectCreationExpressionSyntax
                ? Docs.Group(result)
                : result;
        }
    }
}
