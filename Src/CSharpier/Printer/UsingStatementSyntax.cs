using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingStatementSyntax(UsingStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var docs = new List<Doc>
            {
                this.PrintExtraNewLines(node),
                SyntaxTokens.PrintWithSuffix(node.AwaitKeyword, " "),
                SyntaxTokens.Print(node.UsingKeyword),
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
                SyntaxTokens.Print(node.CloseParenToken)
            };
            if (node.Statement is UsingStatementSyntax)
            {
                docs.Add(Docs.HardLine, SyntaxNodes.Print(node.Statement));
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
                docs.Add(
                    Docs.Indent(
                        Docs.HardLine,
                        SyntaxNodes.Print(node.Statement)
                    )
                );
            }

            return Docs.Concat(docs);
        }
    }
}
