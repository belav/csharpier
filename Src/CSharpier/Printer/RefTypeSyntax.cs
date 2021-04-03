using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRefTypeSyntax(RefTypeSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(
                    node.RefKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.ReadOnlyKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Type)
            );
        }
    }
}
