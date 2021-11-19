namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LocalFunctionStatement
{
    public static Doc Print(LocalFunctionStatementSyntax node)
    {
        return BaseMethodDeclaration.Print(node);
    }
}
