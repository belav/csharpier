using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintQueryContinuationSyntax(QueryContinuationSyntax node)
        {
            return Concat(
                "into",
                " ",
                node.Identifier.Text,
                Line,
                this.PrintQueryBodySyntax(node.Body)
            );
        }
    }
}
