using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNameColonSyntax(NameColonSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.Name.Identifier),
                this.PrintSyntaxToken(node.ColonToken, " ")
            );
        }
    }
}
