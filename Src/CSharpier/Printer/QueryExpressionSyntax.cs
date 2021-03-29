using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryExpressionSyntax(QueryExpressionSyntax node)
        {
            return Indent(
                Line,
                this.PrintFromClauseSyntax(node.FromClause),
                Line,
                this.PrintQueryBodySyntax(node.Body)
            );
        }
    }
}
