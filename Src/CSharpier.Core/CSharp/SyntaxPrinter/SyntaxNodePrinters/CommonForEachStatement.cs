using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class CommonForEachStatement
{
    public static Doc Print(CommonForEachStatementSyntax node, CSharpPrintingContext context)
    {
        var variable = node switch
        {
            ForEachStatementSyntax forEach => Doc.Concat(
                Node.Print(forEach.Type, context),
                " ",
                Token.Print(forEach.Identifier, context)
            ),
            ForEachVariableStatementSyntax forEachVariable => Node.Print(
                forEachVariable.Variable,
                context
            ),
            _ => Doc.Null,
        };

        var docs = Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.AwaitKeyword, context),
                node.AwaitKeyword.RawSyntaxKind() is not SyntaxKind.None ? " " : Doc.Null,
                Token.Print(node.ForEachKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Group(
                    Doc.Indent(
                        Doc.SoftLine,
                        variable,
                        " ",
                        Token.Print(node.InKeyword, context),
                        " ",
                        Node.Print(node.Expression, context)
                    ),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context)
            ),
            OptionalBraces.PrintWithSelfNesting<CommonForEachStatementSyntax>(
                node.Statement,
                context
            )
        );

        return docs;
    }
}
