using System;
using CSharpier.DocTypes;
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
                SyntaxTokens.PrintLeadingTrivia(node.FixedKeyword),
                Docs.Group(
                    SyntaxTokens.PrintWithoutLeadingTrivia(node.FixedKeyword),
                    " ",
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
                    Docs.IfBreak(Docs.Null, Docs.SoftLine)
                ),
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
