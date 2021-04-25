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
            return Docs.Group(
                Modifiers.Print(node.Modifiers),
                this.Print(node.Parameter),
                " ",
                SyntaxTokens.Print(node.ArrowToken),
                node.Body is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntax(blockSyntax)
                    : Docs.Indent(Docs.Line, this.Print(node.Body))
            );
        }
    }
}
