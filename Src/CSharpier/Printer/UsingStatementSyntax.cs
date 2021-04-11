using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingStatementSyntax(UsingStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var parts = new Parts(
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
            );
            if (node.Statement is UsingStatementSyntax)
            {
                parts.Push(HardLine, this.Print(node.Statement));
            }
            else if (node.Statement is BlockSyntax blockSyntax)
            {
                parts.Push(
                    this.PrintBlockSyntaxWithConditionalSpace(
                        blockSyntax,
                        groupId
                    )
                );
            }
            else
            {
                parts.Push(
                    Indent(Concat(HardLine, this.Print(node.Statement)))
                );
            }

            return Concat(parts);
        }
    }
}
