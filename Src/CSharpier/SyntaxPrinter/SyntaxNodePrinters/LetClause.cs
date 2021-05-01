using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class LetClause
    {
        public static Doc Print(LetClauseSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.LetKeyword, " "),
                Token.Print(node.Identifier, " "),
                Token.Print(node.EqualsToken, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
