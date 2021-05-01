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
            var result = Doc.Concat(
                node.Kind() == SyntaxKind.ArrayInitializerExpression
                    ? string.Empty
                    : node.Kind() ==
                            SyntaxKind.ComplexElementInitializerExpression
                            ? Doc.SoftLine
                            : Doc.Line,
                Token.Print(node.OpenBraceToken),
                Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(
                        node.Expressions,
                        this.Print,
                        Doc.Line
                    )
                ),
                Doc.Line,
                Token.Print(node.CloseBraceToken)
            );
            return node.Parent is not ObjectCreationExpressionSyntax
                ? Doc.Group(result)
                : result;
        }
    }
}
