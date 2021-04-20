using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForEachStatementSyntax(ForEachStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var result = Docs.Concat(
                this.PrintExtraNewLines(node),
                SyntaxTokens.PrintWithSuffix(node.AwaitKeyword, " "),
                SyntaxTokens.Print(node.ForEachKeyword),
                " ",
                SyntaxTokens.Print(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    Docs.Indent(
                        Docs.SoftLine,
                        SyntaxNodes.Print(node.Type),
                        " ",
                        SyntaxTokens.Print(node.Identifier),
                        " ",
                        SyntaxTokens.Print(node.InKeyword),
                        " ",
                        SyntaxNodes.Print(node.Expression)
                    ),
                    Docs.SoftLine
                ),
                SyntaxTokens.Print(node.CloseParenToken),
                node.Statement is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntaxWithConditionalSpace(
                            blockSyntax,
                            groupId
                        )
                    : SyntaxNodes.Print(node.Statement)
            );

            return result;
        }
    }
}
