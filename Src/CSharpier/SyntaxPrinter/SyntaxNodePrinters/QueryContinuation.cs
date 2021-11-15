using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryContinuation
{
    public static Doc Print(QueryContinuationSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.IntoKeyword, " "),
            Token.PrintWithSuffix(node.Identifier, Doc.Line),
            QueryBody.Print(node.Body)
        );
    }
}
