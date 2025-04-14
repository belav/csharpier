using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryContinuation
{
    public static Doc Print(QueryContinuationSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.IntoKeyword, " ", context),
            Token.PrintWithSuffix(node.Identifier, Doc.Line, context),
            QueryBody.Print(node.Body, context)
        );
    }
}
