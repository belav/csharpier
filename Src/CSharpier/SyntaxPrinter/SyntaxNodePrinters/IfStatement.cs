using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class IfStatement
    {
        public static Doc Print(IfStatementSyntax node)
        {
            var docs = new List<Doc>();
            if (!(node.Parent is ElseClauseSyntax))
            {
                docs.Add(ExtraNewLines.Print(node));
            }

            var groupId = Guid.NewGuid().ToString();

            docs.Add(
                Token.Print(node.IfKeyword, " "),
                Token.Print(node.OpenParenToken),
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Condition)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken)
            );
            if (node.Statement is BlockSyntax blockSyntax)
            {
                docs.Add(Block.PrintWithConditionalSpace(blockSyntax, groupId));
            }
            else
            {
                // TODO 1 force braces here? make an option?
                docs.Add(
                    Doc.Indent(
                        Doc.Concat(Doc.HardLine, Node.Print(node.Statement))
                    )
                );
            }

            if (node.Else != null)
            {
                docs.Add(Doc.HardLine, Node.Print(node.Else));
            }

            return Doc.Concat(docs);
        }
    }
}
