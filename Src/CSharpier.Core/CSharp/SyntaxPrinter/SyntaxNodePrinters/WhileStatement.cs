using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhileStatement
{
    public static Doc Print(WhileStatementSyntax node, CSharpPrintingContext context)
    {
        var result = Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.WhileKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Group(
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Condition, context)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context)
            ),
            OptionalBraces.PrintWithSelfNesting<WhileStatementSyntax>(node.Statement, context)
        );

        return result;
    }
}
