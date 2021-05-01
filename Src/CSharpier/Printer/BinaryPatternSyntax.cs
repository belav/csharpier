using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBinaryPatternSyntax(BinaryPatternSyntax node)
        {
            return Doc.Concat(
                this.Print(node.Left),
                Doc.Line,
                this.PrintSyntaxToken(
                    node.OperatorToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Right)
            );
        }
    }
}
