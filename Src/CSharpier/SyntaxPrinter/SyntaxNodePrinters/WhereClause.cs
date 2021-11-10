using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class WhereClause
    {
        public static Doc Print(WhereClauseSyntax node)
        {
            return Doc.Group(
                Token.Print(node.WhereKeyword),
                Doc.Indent(Doc.Line, Node.Print(node.Condition))
            );
        }
    }
}
