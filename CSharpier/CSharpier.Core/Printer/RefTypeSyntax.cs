using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintRefTypeSyntax(RefTypeSyntax node)
        {
            return Concat(
                node.RefKeyword.Text,
                node.ReadOnlyKeyword.RawKind != 0 ? " " + node.ReadOnlyKeyword.Text : "",
                " ",
                this.Print(node.Type)
            );
        }
    }
}
