using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDestructorDeclarationSyntax(DestructorDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Add(node.TildeToken.Text);
            parts.Add(node.Identifier.Text);
            parts.Add(this.Print(node.ParameterList));
            parts.Add(this.Print(node.Body));
            return Group(Concat(parts));
        }
    }
}
