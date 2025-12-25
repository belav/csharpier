using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class IfStatement
{
    public static Doc Print(IfStatementSyntax node, PrintingContext context)
    {
        var docs = new DocListBuilder(8);
        if (node.Parent is not ElseClauseSyntax)
        {
            docs.Add(ExtraNewLines.Print(node));
        }

        docs.Add(
            Token.Print(node.IfKeyword, context),
            " ",
            Doc.Group(
                Token.Print(node.OpenParenToken, context),
                Doc.Indent(
                    Doc.IfBreak(Doc.SoftLine, Doc.Null),
                    Node.Print(node.Condition, context)
                ),
                Doc.IfBreak(Doc.SoftLine, Doc.Null)
            ),
            Token.Print(node.CloseParenToken, context),
            OptionalBraces.Print(node.Statement, context)
        );

        if (node.Else != null)
        {
            docs.Add(Doc.HardLine, Node.Print(node.Else, context));
        }

        return Doc.Concat(ref docs);
    }
}
