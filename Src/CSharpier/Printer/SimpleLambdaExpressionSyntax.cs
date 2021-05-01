using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSimpleLambdaExpressionSyntax(
            SimpleLambdaExpressionSyntax node
        ) {
            return Doc.Group(
                Modifiers.Print(node.Modifiers),
                this.Print(node.Parameter),
                " ",
                Token.Print(node.ArrowToken),
                node.Body is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntax(blockSyntax)
                    : Doc.Indent(Doc.Line, this.Print(node.Body))
            );
        }
    }
}
