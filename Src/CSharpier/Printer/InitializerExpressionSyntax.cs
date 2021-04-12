using System.Linq;
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
                    : node.Kind()
                            == SyntaxKind.ComplexElementInitializerExpression
                            ? Docs.SoftLine
                            : Docs.Line,
                this.PrintSyntaxToken(node.OpenBraceToken),
                Docs.Indent(
                    Docs.Line,
                    this.PrintSeparatedSyntaxList(
                        node.Expressions,
                        this.Print,
                        Docs.Line
                    )
                ),
                Docs.Line,
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
            return node.Parent is not ObjectCreationExpressionSyntax
                ? Docs.Group(result)
                : result;
        }
    }
}
