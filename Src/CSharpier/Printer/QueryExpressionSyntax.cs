using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryExpressionSyntax(QueryExpressionSyntax node)
        {
            return Doc.Indent(
                Doc.Line,
                this.PrintFromClauseSyntax(node.FromClause),
                Doc.Line,
                this.PrintQueryBodySyntax(node.Body)
            );
        }
    }
}
