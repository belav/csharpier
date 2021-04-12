using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryExpressionSyntax(QueryExpressionSyntax node)
        {
            return Docs.Indent(
                Docs.Line,
                this.PrintFromClauseSyntax(node.FromClause),
                Docs.Line,
                this.PrintQueryBodySyntax(node.Body)
            );
        }
    }
}
