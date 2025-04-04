namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UsingStatement
{
    public static Doc Print(UsingStatementSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null]);
        docs.Append(ExtraNewLines.Print(node));
        docs.Append(
            Doc.Group(
                Token.Print(node.AwaitKeyword, context),
                node.AwaitKeyword.RawSyntaxKind() != SyntaxKind.None ? " " : Doc.Null,
                Token.Print(node.UsingKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Group(
                    Doc.Indent(
                        Doc.SoftLine,
                        node.Declaration != null
                            ? VariableDeclaration.Print(node.Declaration, context)
                            : Doc.Null,
                        node.Expression != null ? Node.Print(node.Expression, context) : Doc.Null
                    ),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context),
                Doc.IfBreak(Doc.Null, Doc.SoftLine)
            )
        );

        if (node.Statement is UsingStatementSyntax)
        {
            docs.Append(Doc.HardLine, Node.Print(node.Statement, context));
        }
        else if (node.Statement is BlockSyntax blockSyntax)
        {
            docs.Append(Block.Print(blockSyntax, context));
        }
        else
        {
            docs.Append(Doc.Indent(Doc.HardLine, Node.Print(node.Statement, context)));
        }

        return Doc.Concat(ref docs);
    }
}
