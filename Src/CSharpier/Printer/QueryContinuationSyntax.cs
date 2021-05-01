using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryContinuationSyntax(QueryContinuationSyntax node)
        {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.IntoKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.Identifier, Doc.Line),
                this.PrintQueryBodySyntax(node.Body)
            );
        }
    }
}
