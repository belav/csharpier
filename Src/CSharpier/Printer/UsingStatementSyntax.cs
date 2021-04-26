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
                ? SyntaxTokens.PrintLeadingTrivia(node.AwaitKeyword)
                : SyntaxTokens.PrintLeadingTrivia(node.UsingKeyword);

            var docs = new List<Doc>
            {
                this.PrintExtraNewLines(node),
                leadingTrivia,
                Docs.Group(
                    SyntaxTokens.PrintWithoutLeadingTrivia(node.AwaitKeyword),
                    node.AwaitKeyword.Kind() != SyntaxKind.None
                        ? " "
                        : Docs.Null,
                    node.AwaitKeyword.Kind() == SyntaxKind.None
                        ? SyntaxTokens.PrintWithoutLeadingTrivia(
                                node.UsingKeyword
                            )
                        : SyntaxTokens.Print(node.UsingKeyword),
                    " ",
                    SyntaxTokens.Print(node.OpenParenToken),
                    Docs.GroupWithId(
                        groupId,
                        Docs.Indent(
                            Docs.SoftLine,
                            node.Declaration != null
                                ? this.PrintVariableDeclarationSyntax(
                                        node.Declaration
                                    )
                                : Doc.Null,
                            node.Expression != null
                                ? this.Print(node.Expression)
                                : Doc.Null
                        ),
                        Docs.SoftLine
                    ),
                    SyntaxTokens.Print(node.CloseParenToken),
                    Docs.IfBreak(Docs.Null, Docs.SoftLine)
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
