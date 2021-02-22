using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefTypeSyntax(RefTypeSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.RefKeyword, " "),
                this.PrintSyntaxToken(node.ReadOnlyKeyword, " "),
                this.Print(node.Type));
        }
    }
}
