using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentSyntax(ArgumentSyntax node)
        {
            var parts = new Parts();
            if (node.NameColon != null)
            {
                parts.Push(this.PrintNameColonSyntax(node.NameColon));
            }

            parts.Push(this.PrintSyntaxToken(node.RefKindKeyword, " "));
            parts.Push(this.Print(node.Expression));
            return Concat(parts);
        }
    }
}
