using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBinaryPatternSyntax(BinaryPatternSyntax node)
        {
            return Concat(
                this.Print(node.Left),
                Docs.Line,
                this.PrintSyntaxToken(
                    node.OperatorToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Right)
            );
        }
    }
}
