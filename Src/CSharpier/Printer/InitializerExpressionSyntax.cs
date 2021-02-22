using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintInitializerExpressionSyntax(
            InitializerExpressionSyntax node)
        {
            return Group(
                Concat(
                    node.Kind() == SyntaxKind.ArrayInitializerExpression
                        ? ""
                        : node.Kind() == SyntaxKind.ComplexElementInitializerExpression
                            ? SoftLine
                            : Line,
                    this.PrintSyntaxToken(node.OpenBraceToken),
                    Indent(
                        Concat(
                            Line,
                            this.PrintSeparatedSyntaxList(
                                node.Expressions,
                                this.Print,
                                Line))),
                    Line,
                    this.PrintSyntaxToken(node.CloseBraceToken)));
        }
    }
}
