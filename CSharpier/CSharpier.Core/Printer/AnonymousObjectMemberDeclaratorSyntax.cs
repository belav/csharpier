using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectMemberDeclaratorSyntax(AnonymousObjectMemberDeclaratorSyntax node)
        {
            var parts = new Parts();
            if (node.NameEquals != null) {
                parts.Add(this.PrintIdentifierNameSyntax(node.NameEquals.Name));
                parts.Add(" = ");
            }
            parts.Add(this.Print(node.Expression));
            return Concat(parts);
        }
    }
}
