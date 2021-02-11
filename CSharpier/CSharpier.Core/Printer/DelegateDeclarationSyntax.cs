using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDelegateDeclarationSyntax(DelegateDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(node.DelegateKeyword.Text, " ");
            parts.Push(this.Print(node.ReturnType));
            parts.Push(" ", node.Identifier.Text);
            if (node.TypeParameterList != null) {
                parts.Push(this.Print(node.TypeParameterList));
            }
            parts.Push(this.Print(node.ParameterList));
            this.PrintConstraintClauses(node, node.ConstraintClauses, parts);
            parts.Push(";");
            return Concat(parts);
        }
    }
}
