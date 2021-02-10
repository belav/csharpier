using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintVariableDeclarationSyntax(VariableDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.Print(node.Type),
                " ",
                Join(
                    String(", "),
                    node.Variables.Select(this.PrintVariableDeclaratorSyntax)
                ));

            return Concat(parts);
        }
    }
}
