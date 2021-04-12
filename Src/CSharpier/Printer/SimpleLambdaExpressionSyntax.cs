using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSimpleLambdaExpressionSyntax(
            SimpleLambdaExpressionSyntax node
        ) {
            return Docs.Group(
                this.PrintModifiers(node.Modifiers),
                this.Print(node.Parameter),
                Docs.SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.ArrowToken),
                node.Body is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntax(blockSyntax)
                    : Docs.Indent(Docs.Line, this.Print(node.Body))
            );
        }
    }
}
