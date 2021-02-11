using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDelegateDeclarationSyntax(DelegateDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintSyntaxToken(node.DelegateKeyword, " "));
            parts.Push(this.Print(node.ReturnType));
            parts.Push(" ", this.PrintSyntaxToken(node.Identifier));
            if (node.TypeParameterList != null) {
                parts.Push(this.Print(node.TypeParameterList));
            }
            parts.Push(this.Print(node.ParameterList));
            this.PrintConstraintClauses(node, node.ConstraintClauses, parts);
            parts.Push(this.PrintSyntaxToken(node.SemicolonToken));
            return Concat(parts);
        }
    }
}
