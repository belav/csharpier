namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryContinuation
{
    public static Doc Print(QueryContinuationSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.IntoKeyword, " ", context),
            Token.Print(node.Identifier, context),
            QueryBody.Print(node.Body, context)
        );
    }
}
