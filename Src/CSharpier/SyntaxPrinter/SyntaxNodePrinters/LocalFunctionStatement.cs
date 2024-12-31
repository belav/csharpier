namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LocalFunctionStatement
{
    public static Doc Print(LocalFunctionStatementSyntax node, PrintingContext context)
    {
        return BaseMethodDeclaration.Print(node, context);
    }
}
