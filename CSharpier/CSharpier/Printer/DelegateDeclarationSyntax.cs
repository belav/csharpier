using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDelegateDeclarationSyntax(DelegateDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(node.DelegateKeyword.Text, String(" "));
            parts.Push(this.Print(node.ReturnType));
            parts.Push(String(" "), node.Identifier.Text);
            if (NotNull(node.TypeParameterList)) {
                parts.Push(this.Print(node.TypeParameterList));
            }
            parts.Push(this.Print(node.ParameterList));
            this.PrintConstraintClauses(node.ConstraintClauses, parts);
            parts.Push(String(";"));
            return Concat(parts);
        }
    }
}
