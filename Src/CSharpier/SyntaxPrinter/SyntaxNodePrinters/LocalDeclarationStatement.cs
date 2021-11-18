namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LocalDeclarationStatement
{
    public static Doc Print(LocalDeclarationStatementSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.AwaitKeyword, " "),
            Token.PrintWithSuffix(node.UsingKeyword, " "),
            Modifiers.Print(node.Modifiers),
            VariableDeclaration.Print(node.Declaration),
            Token.Print(node.SemicolonToken)
        );
    }
}
