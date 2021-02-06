using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefTypeSyntax(RefTypeSyntax node)
        {
            return Concat(
                node.RefKeyword.Text,
                node.ReadOnlyKeyword.RawKind != 0 ? " " + node.ReadOnlyKeyword.Text : "",
                String(" "),
                this.Print(node.Type)
            );
        }
    }
}
