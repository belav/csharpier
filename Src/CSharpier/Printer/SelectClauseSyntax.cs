using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSelectClauseSyntax(SelectClauseSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.SelectKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Expression)
            );
        }
    }
}
