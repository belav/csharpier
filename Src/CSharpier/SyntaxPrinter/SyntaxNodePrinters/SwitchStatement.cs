using System;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchStatement
{
    public static Doc Print(SwitchStatementSyntax node)
    {
        Doc sections =
            node.Sections.Count == 0
                ? " "
                : Doc.Concat(
                      Doc.Indent(
                          Doc.Concat(
                              Doc.HardLine,
                              Doc.Join(Doc.HardLine, node.Sections.Select(SwitchSection.Print))
                          )
                      ),
                      Doc.HardLine
                  );

        DocUtilities.RemoveInitialDoubleHardLine(sections);

        var groupId = Guid.NewGuid().ToString();

        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintLeadingTrivia(node.SwitchKeyword),
            Doc.Group(
                Token.PrintWithoutLeadingTrivia(node.SwitchKeyword),
                " ",
                Token.Print(node.OpenParenToken),
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Expression)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken),
                Doc.IfBreak(" ", Doc.Line, groupId),
                Token.Print(node.OpenBraceToken),
                sections,
                Token.Print(node.CloseBraceToken)
            )
        );
    }
}
