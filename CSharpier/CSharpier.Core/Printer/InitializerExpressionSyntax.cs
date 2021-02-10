using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintInitializerExpressionSyntax(InitializerExpressionSyntax node)
        {
            return Group(
                Concat(
                    node.Kind() == SyntaxKind.ArrayInitializerExpression
                        ? ""
                        : node.Kind() == SyntaxKind.ComplexElementInitializerExpression
                            ? SoftLine
                            : Line,
                    "{",
                    Indent(Concat(Line, this.PrintCommaList(node.Expressions.Select(this.Print)))),
                    Line,
                    "}"
                )
            );
        }
    }
}