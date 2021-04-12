using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUnaryPatternSyntax(UnaryPatternSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.OperatorToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Pattern)
            );
        }
    }
}
