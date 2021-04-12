using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorConstraintSyntax(
            ConstructorConstraintSyntax node
        ) {
            return Docs.Concat(
                this.PrintSyntaxToken(node.NewKeyword),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
