using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class QueryContinuation
    {
        public static Doc Print(QueryContinuationSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.IntoKeyword, " "),
                Token.Print(node.Identifier, Doc.Line),
                QueryBody.Print(node.Body)
            );
        }
    }
}
