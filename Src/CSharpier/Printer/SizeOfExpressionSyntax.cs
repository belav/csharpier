using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSizeOfExpressionSyntax(SizeOfExpressionSyntax node)
        {
            return Docs.Concat(
                SyntaxTokens.Print(node.Keyword),
                SyntaxTokens.Print(node.OpenParenToken),
                this.Print(node.Type),
                SyntaxTokens.Print(node.CloseParenToken)
            );
        }
    }
}
