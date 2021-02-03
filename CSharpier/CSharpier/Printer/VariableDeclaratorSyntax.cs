using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclaratorSyntax(VariableDeclaratorSyntax node)
        {
            var parts = new Parts();
            parts.Push(node.Identifier.Text);
            if (NotNull(node.ArgumentList)) {
                parts.Push(this.PrintBracketedArgumentListSyntax(node.ArgumentList));
            }
            if (NotNull(node.Initializer)) {
                parts.Push(this.PrintEqualsValueClauseSyntax(node.Initializer));
            }
            return Concat(parts);
        }
    }
}
