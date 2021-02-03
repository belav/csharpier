using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExternAliasDirectiveSyntax(ExternAliasDirectiveSyntax node)
        {
            return Concat(
                String(node.ExternKeyword.Text),
                String(" "),
                String(node.AliasKeyword.Text),
                String(" "),
                String(node.Identifier.Text),
                String(";")
            );
        }
    }
}
