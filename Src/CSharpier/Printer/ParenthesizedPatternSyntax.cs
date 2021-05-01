using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedPatternSyntax(
            ParenthesizedPatternSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                this.Print(node.Pattern),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
