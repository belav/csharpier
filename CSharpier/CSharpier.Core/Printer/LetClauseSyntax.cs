using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintLetClauseSyntax(LetClauseSyntax node)
        {
            return Concat("let ", node.Identifier.Text, " = ", this.Print(node.Expression));
        }
    }
}
