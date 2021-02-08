using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorDeclarationSyntax(ConstructorDeclarationSyntax node)
        {
            this.printNewLinesInLeadingTrivia.Push(true);
            var parts = new Parts();
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(node.Identifier.Text);
            this.printNewLinesInLeadingTrivia.Pop();
            parts.Add(this.PrintParameterListSyntax(node.ParameterList));
            parts.Add(this.PrintBlockSyntax(node.Body));
            return Group(Concat(parts));
        }
    }
}
