using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDestructorDeclarationSyntax(DestructorDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintSyntaxToken(node.TildeToken));
            parts.Push(this.PrintSyntaxToken(node.Identifier));
            parts.Push(this.Print(node.ParameterList));
            parts.Push(this.Print(node.Body));
            return Group(Concat(parts));
        }
    }
}
