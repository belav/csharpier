using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNameEqualsSyntax(NameEqualsSyntax node)
        {
            return Docs.Concat(
                this.Print(node.Name),
                " ",
                this.PrintSyntaxToken(
                    node.EqualsToken,
                    afterTokenIfNoTrailing: " "
                )
            );
        }
    }
}
