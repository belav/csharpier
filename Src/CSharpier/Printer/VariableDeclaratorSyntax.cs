using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclaratorSyntax(VariableDeclaratorSyntax node)
        {
            var parts = new Parts(this.PrintSyntaxToken(node.Identifier));
            if (node.ArgumentList != null)
            {
                parts.Push(
                    this.PrintBracketedArgumentListSyntax(node.ArgumentList)
                );
            }
            if (node.Initializer != null)
            {
                parts.Push(this.PrintEqualsValueClauseSyntax(node.Initializer));
            }
            return Concat(parts);
        }
    }
}
