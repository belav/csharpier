using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintUnaryPatternSyntax(UnaryPatternSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.OperatorToken, " "), this.Print(node.Pattern));
        }
    }
}
