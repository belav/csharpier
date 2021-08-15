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
                Doc.Indent(Doc.SoftLine, Node.Print(node.Pattern)),
                Doc.SoftLine,
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
