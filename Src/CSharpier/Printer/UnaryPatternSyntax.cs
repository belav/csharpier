using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUnaryPatternSyntax(UnaryPatternSyntax node)
        {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.OperatorToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Pattern)
            );
        }
    }
}
