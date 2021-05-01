using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class WhereClause
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
