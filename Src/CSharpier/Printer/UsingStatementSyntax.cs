using System;
using System.Collections.Generic;
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
                this.PrintSyntaxToken(
                    node.AwaitKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.UsingKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    node.Declaration != null
                        ? this.PrintVariableDeclarationSyntax(node.Declaration)
                        : Doc.Null,
                    node.Expression != null
                        ? this.Print(node.Expression)
                        : Doc.Null,
                    Docs.SoftLine
                ),
                this.PrintSyntaxToken(node.CloseParenToken)
            };
            if (node.Statement is UsingStatementSyntax)
            {
                docs.Add(Docs.HardLine, this.Print(node.Statement));
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
                    Docs.Indent(Docs.HardLine, this.Print(node.Statement))
                );
            }

            return Docs.Concat(docs);
        }
    }
}
