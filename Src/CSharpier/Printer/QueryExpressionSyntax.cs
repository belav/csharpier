using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryExpressionSyntax(QueryExpressionSyntax node)
        {
            return Concat(
                this.PrintFromClauseSyntax(node.FromClause),
                Indent(Concat(Line, this.PrintQueryBodySyntax(node.Body))));
        }
    }
}
