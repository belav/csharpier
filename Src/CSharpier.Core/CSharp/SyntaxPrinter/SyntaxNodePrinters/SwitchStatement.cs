using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchStatement
{
    public static Doc Print(SwitchStatementSyntax node, CSharpPrintingContext context)
    {
        var sections =
            node.Sections.Count == 0
                ? " "
                : Doc.Concat(
                    Doc.Indent(
                        Doc.Concat(
                            Doc.HardLine,
                            Doc.Join(Doc.HardLine, node.Sections, SwitchSection.Print, context)
                        )
                    ),
                    Doc.HardLine
                );

        DocUtilities.RemoveInitialDoubleHardLine(sections);

        var groupId = Doc.NextGroupId();

        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintLeadingTrivia(node.SwitchKeyword, context),
            Doc.Group(
                Token.PrintWithoutLeadingTrivia(node.SwitchKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Expression, context)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context),
                node.Sections.Count == 0 ? " " : Doc.Line,
                Token.Print(node.OpenBraceToken, context),
                sections,
                Token.Print(node.CloseBraceToken, context)
            )
        );
    }
}
