using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ParenthesizedPattern
    {
        public static Doc Print(ParenthesizedPatternSyntax node)
        {
            return Doc.Group(
                Token.Print(node.OpenParenToken),
                Node.Print(node.Pattern),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
