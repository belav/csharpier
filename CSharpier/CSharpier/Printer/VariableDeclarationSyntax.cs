using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclarationSyntax(VariableDeclarationSyntax node)
        {
            var parts = new Parts();
            // TODO how do we really know this? if a LocalDeclarationStatement has await/using, this may be true already
            var printedExtraNewLines = false;
            this.PrintLeadingTrivia(node.Type.GetLeadingTrivia(), parts, ref printedExtraNewLines);
            parts.Push(this.Print(node.Type),
                " ",
                Join(
                    String(", "),
                    node.Variables.Select(this.PrintVariableDeclaratorSyntax)
                ));

            return Concat(parts);
        }
    }
}
