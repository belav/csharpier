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
                ? Token.PrintLeadingTrivia(node.AwaitKeyword)
                : Token.PrintLeadingTrivia(node.ForEachKeyword);

            var docs = Doc.Concat(
                ExtraNewLines.Print(node),
                leadingTrivia,
                Doc.Group(
                    Token.PrintWithoutLeadingTrivia(node.AwaitKeyword),
                    node.AwaitKeyword.Kind() != SyntaxKind.None
                        ? " "
                        : Doc.Null,
                    node.AwaitKeyword.Kind() == SyntaxKind.None
                        ? Token.PrintWithoutLeadingTrivia(node.ForEachKeyword)
                        : Token.Print(node.ForEachKeyword),
                    " ",
                    Token.Print(node.OpenParenToken),
                    Doc.GroupWithId(
                        groupId,
                        Doc.Indent(
                            Doc.SoftLine,
                            Node.Print(node.Type),
                            " ",
                            Token.Print(node.Identifier),
                            " ",
                            Token.Print(node.InKeyword),
                            " ",
                            Node.Print(node.Expression)
                        ),
                        Doc.SoftLine
                    ),
                    Token.Print(node.CloseParenToken),
                    Doc.IfBreak(Doc.Null, Doc.SoftLine)
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
