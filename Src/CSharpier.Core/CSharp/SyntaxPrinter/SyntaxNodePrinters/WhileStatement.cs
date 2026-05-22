using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhileStatement
{
    public static Doc Print(WhileStatementSyntax node, PrintingContext context)
    {
        var result = Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.WhileKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Indent(Node.Print(node.Condition, context)),
                Token.Print(node.CloseParenToken, context)
            ),
            node.Statement switch
            {
                WhileStatementSyntax => Doc.Group(
                    Doc.HardLine,
                    Node.Print(node.Statement, context)
                ),
                _ => OptionalBraces.Print(node.Statement, context),
            }
        );

        return result;
    }
}
