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
                Token.PrintLeadingTrivia(node.IfKeyword),
                Doc.Group(
                    Token.PrintWithoutLeadingTrivia(node.IfKeyword),
                    " ",
                    Token.Print(node.OpenParenToken),
                    Doc.GroupWithId(
                        groupId,
                        Doc.Indent(Doc.SoftLine, this.Print(node.Condition)),
                        Doc.SoftLine
                    ),
                    Token.Print(node.CloseParenToken),
                    Doc.IfBreak(Doc.Null, Doc.SoftLine)
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
