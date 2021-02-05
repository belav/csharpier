using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorDeclarationSyntax(ConstructorDeclarationSyntax node)
        {
            var parts = new Parts();
            //this.PrintExtraNewLines(node, String("attributeLists"), String("modifiers"), String("identifier"));
            // TODO printLeadingComments(node, parts, String("attributeLists"), String("modifiers"), String("identifier"));
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(node.Identifier.Text);
            parts.Add(this.PrintParameterListSyntax(node.ParameterList));
            parts.Add(this.PrintBlockSyntax(node.Body));
            return Group(Concat(parts));
        }
    }
}
