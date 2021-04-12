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
            var result = Concat(
                node.Kind() == SyntaxKind.ArrayInitializerExpression
                    ? string.Empty
                    : node.Kind()
                            == SyntaxKind.ComplexElementInitializerExpression
                            ? SoftLine
                            : Line,
                this.PrintSyntaxToken(node.OpenBraceToken),
                Indent(
                    Line,
                    this.PrintSeparatedSyntaxList(
                        node.Expressions,
                        this.Print,
                        Line
                    )
                ),
                Line,
                this.PrintSyntaxToken(node.CloseBraceToken)
            );
            return node.Parent is not ObjectCreationExpressionSyntax
                ? Group(result)
                : result;
        }
    }
}
