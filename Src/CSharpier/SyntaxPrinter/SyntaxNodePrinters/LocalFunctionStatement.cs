namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LocalFunctionStatement
{
    public static Doc Print(LocalFunctionStatementSyntax node, FormattingContext context)
    {
        return BaseMethodDeclaration.Print(node, context);
    }
}
