using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCatchClauseSyntax(CatchClauseSyntax node)
        {
            var parts = new Parts();
            parts.Add("catch");
            if (node.Declaration != null)
            {
                parts.Add(Concat(
                    " (",
                    this.Print(node.Declaration.Type),
                    node.Declaration.Identifier.RawKind != 0 ? " " : "",
                    node.Declaration.Identifier.Text,
                    ")"));
            }

            if (node.Filter != null)
            {
                parts.Add(Concat(" when (", this.Print(node.Filter.FilterExpression), ")"));
            }
            
            parts.Add(this.PrintBlockSyntax(node.Block));
            return Concat(parts);
        }
    }
}
