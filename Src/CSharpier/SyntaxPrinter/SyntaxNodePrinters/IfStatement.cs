using CSharpier.Utilities;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class IfStatement
{
    public static Doc Print(IfStatementSyntax node)
    {
        var docs = new List<Doc>();
        if (node.Parent is not ElseClauseSyntax)
        {
            docs.Add(ExtraNewLines.Print(node));
        }

        docs.Add(
            Doc.Group(
                Token.Print(node.IfKeyword),
                " ",
                Token.Print(node.OpenParenToken),
                Doc.Group(Doc.Indent(Doc.SoftLine, Node.Print(node.Condition)), Doc.SoftLine),
                Token.Print(node.CloseParenToken)
            ),
            OptionalBraces.Print(node.Statement)
        );

        if (node.Else != null)
        {
            docs.Add(Doc.HardLine, Node.Print(node.Else));
        }

        return Doc.Concat(docs);
    }
}
