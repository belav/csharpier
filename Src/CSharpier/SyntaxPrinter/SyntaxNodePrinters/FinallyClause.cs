using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class FinallyClause
    {
        public static Doc Print(FinallyClauseSyntax node)
        {
            return Doc.Concat(Token.Print(node.FinallyKeyword), Node.Print(node.Block));
        }
    }
}
