using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingStatementSyntax(UsingStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var leadingTrivia = node.AwaitKeyword.Kind() != SyntaxKind.None
                ? Token.PrintLeadingTrivia(node.AwaitKeyword)
                : Token.PrintLeadingTrivia(node.UsingKeyword);

            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                leadingTrivia,
                Doc.Group(
                    Token.PrintWithoutLeadingTrivia(node.AwaitKeyword),
                    node.AwaitKeyword.Kind() != SyntaxKind.None
                        ? " "
                        : Doc.Null,
                    node.AwaitKeyword.Kind() == SyntaxKind.None
                        ? Token.PrintWithoutLeadingTrivia(node.UsingKeyword)
                        : Token.Print(node.UsingKeyword),
                    " ",
                    Token.Print(node.OpenParenToken),
                    Doc.GroupWithId(
                        groupId,
                        Doc.Indent(
                            Doc.SoftLine,
                            node.Declaration != null
                                ? this.PrintVariableDeclarationSyntax(
                                        node.Declaration
                                    )
                                : Doc.Null,
                            node.Expression != null
                                ? this.Print(node.Expression)
                                : Doc.Null
                        ),
                        Doc.SoftLine
                    ),
                    Token.Print(node.CloseParenToken),
                    Doc.IfBreak(Doc.Null, Doc.SoftLine)
                )
            };
            if (node.Statement is UsingStatementSyntax)
            {
                docs.Add(Doc.HardLine, Node.Print(node.Statement));
            }
            else if (node.Statement is BlockSyntax blockSyntax)
            {
                docs.Add(
                    this.PrintBlockSyntaxWithConditionalSpace(
                        blockSyntax,
                        groupId
                    )
                );
            }
            else
            {
                docs.Add(Doc.Indent(Doc.HardLine, Node.Print(node.Statement)));
            }

            return Doc.Concat(docs);
        }
    }
}
