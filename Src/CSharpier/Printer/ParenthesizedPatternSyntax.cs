using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedPatternSyntax(
            ParenthesizedPatternSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Pattern),
                this.PrintSyntaxToken(node.CloseParenToken));
        }
    }
}
