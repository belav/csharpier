using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryContinuationSyntax(QueryContinuationSyntax node)
        {
            return Concat(
                String("into"),
                String(" "),
                String(node.Identifier.Text),
                Line,
                this.PrintQueryBodySyntax(node.Body)
            );
        }
    }
}
