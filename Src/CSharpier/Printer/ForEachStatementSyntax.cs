using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForEachStatementSyntax(ForEachStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var leadingTrivia = node.AwaitKeyword.Kind() != SyntaxKind.None
                ? SyntaxTokens.PrintLeadingTrivia(node.AwaitKeyword)
                : SyntaxTokens.PrintLeadingTrivia(node.ForEachKeyword);

            var docs = Docs.Concat(
                this.PrintExtraNewLines(node),
                leadingTrivia,
                Docs.Group(
                    SyntaxTokens.PrintWithoutLeadingTrivia(node.AwaitKeyword),
                    node.AwaitKeyword.Kind() != SyntaxKind.None
                        ? " "
                        : Docs.Null,
                    node.AwaitKeyword.Kind() == SyntaxKind.None
                        ? SyntaxTokens.PrintWithoutLeadingTrivia(
                                node.ForEachKeyword
                            )
                        : SyntaxTokens.Print(node.ForEachKeyword),
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
                    Docs.IfBreak(Docs.Null, Docs.SoftLine)
                ),
                node.Statement is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntaxWithConditionalSpace(
                            blockSyntax,
                            groupId
                        )
                    : Node.Print(node.Statement)
            );

            return docs;
        }
    }
}
