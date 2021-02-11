using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDestructorDeclarationSyntax(DestructorDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(node.TildeToken.Text);
            parts.Push(node.Identifier.Text);
            parts.Push(this.Print(node.ParameterList));
            parts.Push(this.Print(node.Body));
            return Group(Concat(parts));
        }
    }
}
