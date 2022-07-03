namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeParameter
{
    public static Doc Print(TypeParameterSyntax node, FormattingContext context)
    {
        var hasAttribute = node.AttributeLists.Any();
        var groupId = hasAttribute ? Guid.NewGuid().ToString() : string.Empty;

        var result = Doc.Concat(
            AttributeLists.Print(node, node.AttributeLists, context),
            hasAttribute ? Doc.IndentIfBreak(Doc.Line, groupId) : Doc.Null,
            node.VarianceKeyword.RawSyntaxKind() != SyntaxKind.None
                ? Token.PrintWithSuffix(node.VarianceKeyword, " ", context)
                : Doc.Null,
            Token.Print(node.Identifier, context)
        );

        return hasAttribute ? Doc.GroupWithId(groupId, result) : result;
    }
}
