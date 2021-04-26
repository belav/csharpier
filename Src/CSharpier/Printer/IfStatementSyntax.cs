using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
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
                docs.Add(ExtraNewLines.Print(node));
            }

            var groupId = Guid.NewGuid().ToString();

            docs.Add(
                SyntaxTokens.PrintLeadingTrivia(node.IfKeyword),
                Docs.Group(
                    SyntaxTokens.PrintWithoutLeadingTrivia(node.IfKeyword),
                    " ",
                    SyntaxTokens.Print(node.OpenParenToken),
                    Docs.GroupWithId(
                        groupId,
                        Docs.Indent(Docs.SoftLine, this.Print(node.Condition)),
                        Docs.SoftLine
                    ),
                    SyntaxTokens.Print(node.CloseParenToken),
                    Docs.IfBreak(Docs.Null, Docs.SoftLine)
                )
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
                    Doc.Indent(
                        Doc.Concat(Doc.HardLine, this.Print(node.Statement))
                    )
                );
            }

            if (node.Else != null)
            {
                docs.Add(Doc.HardLine, this.Print(node.Else));
            }

            return Doc.Concat(docs);
        }
    }
}
