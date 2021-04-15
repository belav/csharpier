using System;
using System.Collections.Generic;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIfStatementSyntax(IfStatementSyntax node)
        {
            var docs = new List<Doc>();
            if (!(node.Parent is ElseClauseSyntax))
            {
                docs.Add(this.PrintExtraNewLines(node));
            }

            var groupId = Guid.NewGuid().ToString();

            docs.Add(
                this.PrintSyntaxToken(
                    node.IfKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SyntaxTokens.Print(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    Docs.Indent(Docs.SoftLine, this.Print(node.Condition)),
                    Docs.SoftLine
                ),
                SyntaxTokens.Print(node.CloseParenToken)
            );
            if (node.Statement is BlockSyntax blockSyntax)
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
                // TODO 1 force braces here? make an option?
                docs.Add(
                    Docs.Indent(
                        Docs.Concat(Docs.HardLine, this.Print(node.Statement))
                    )
                );
            }

            if (node.Else != null)
            {
                docs.Add(Docs.HardLine, this.Print(node.Else));
            }

            return Docs.Concat(docs);
        }
    }
}
