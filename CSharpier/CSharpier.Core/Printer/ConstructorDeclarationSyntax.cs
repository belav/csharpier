using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConstructorDeclarationSyntax(ConstructorDeclarationSyntax node)
        {
            this.printNewLinesInLeadingTrivia.Push(true);
            var parts = new Parts();
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintLeadingTrivia(node.Identifier));
            parts.Push(node.Identifier.Text);
            parts.Push(this.PrintTrailingTrivia(node.Identifier));
            this.printNewLinesInLeadingTrivia.Pop();
            parts.Push(this.PrintParameterListSyntax(node.ParameterList));
            parts.Push(this.PrintBlockSyntax(node.Body));
            return Group(Concat(parts));
        }
    }
}
