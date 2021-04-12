using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBracketedParameterListSyntax(
            BracketedParameterListSyntax node
        ) {
            return Docs.Concat(
                this.PrintSyntaxToken(node.OpenBracketToken),
                this.PrintSeparatedSyntaxList(
                    node.Parameters,
                    this.PrintParameterSyntax,
                    " "
                ),
                this.PrintSyntaxToken(node.CloseBracketToken)
            );
        }
    }
}
