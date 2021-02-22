using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryContinuationSyntax(QueryContinuationSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.IntoKeyword, " "),
                this.PrintSyntaxToken(node.Identifier, Line),
                this.PrintQueryBodySyntax(node.Body));
        }
    }
}
