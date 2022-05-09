namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SingleVariableDesignation
{
    public static Doc Print(SingleVariableDesignationSyntax node, FormattingContext context)
    {
        return Token.Print(node.Identifier, context);
    }
}
