using System;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFixedStatementSyntax(FixedStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.FixedKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SyntaxTokens.Print(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    Docs.Indent(
                        Docs.SoftLine,
                        SyntaxNodes.Print(node.Declaration)
                    ),
                    Docs.SoftLine
                ),
                SyntaxTokens.Print(node.CloseParenToken),
                node.Statement is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntaxWithConditionalSpace(
                            blockSyntax,
                            groupId
                        )
                    : this.Print(node.Statement)
            );
        }
    }
}
