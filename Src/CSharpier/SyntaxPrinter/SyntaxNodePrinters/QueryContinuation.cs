namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryContinuation
{
    public static Doc Print(QueryContinuationSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.IntoKeyword, " ", context),
            Token.PrintWithSuffix(node.Identifier, Doc.Line, context),
            QueryBody.Print(node.Body, context)
        );
    }
}
