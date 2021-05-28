using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class QueryExpression
    {
        public static Doc Print(QueryExpressionSyntax node)
        {
            return Doc.Concat(
                FromClause.Print(node.FromClause),
                Doc.Line,
                QueryBody.Print(node.Body)
            );
        }
    }
}
