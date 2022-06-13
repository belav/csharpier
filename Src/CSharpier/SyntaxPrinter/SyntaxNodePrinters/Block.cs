namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Block
{
    public static Doc Print(BlockSyntax node, FormattingContext context)
    {
        if (
            node.Statements.Count == 0
            && node.Parent is MethodDeclarationSyntax
            && !Token.HasComments(node.CloseBraceToken)
        )
        {
            return Doc.Concat(
                " ",
                Token.Print(node.OpenBraceToken, context),
                " ",
                Token.Print(node.CloseBraceToken, context)
            );
        }

        Doc statementSeparator =
            node.Parent is AccessorDeclarationSyntax && node.Statements.Count <= 1
                ? Doc.Line
                : Doc.HardLine;

        Doc innerDoc = Doc.Null;

        if (node.Statements.Count > 0)
        {
            var statements = CSharpierIgnore.PrintNodesRespectingRangeIgnore(
                node.Statements,
                context
            );

            innerDoc = Doc.Indent(statementSeparator, Doc.Join(statementSeparator, statements));

            DocUtilities.RemoveInitialDoubleHardLine(innerDoc);
        }

        var result = Doc.Group(
            node.Parent is ParenthesizedLambdaExpressionSyntax or BlockSyntax ? Doc.Null : Doc.Line,
            Token.Print(node.OpenBraceToken, context),
            node.Statements.Count == 0 ? " " : Doc.Concat(innerDoc, statementSeparator),
            Token.Print(node.CloseBraceToken, context)
        );

        return node.Parent is BlockSyntax ? Doc.Concat(ExtraNewLines.Print(node), result) : result;
    }
}
