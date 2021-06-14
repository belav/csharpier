using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
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
                Doc.Group(
                    Token.PrintLeadingTrivia(node.IfKeyword),
                    Doc.Group(
                        Token.PrintWithoutLeadingTrivia(node.IfKeyword),
                        " ",
                        Token.Print(node.OpenParenToken),
                        Doc.GroupWithId(
                            groupId,
                            Doc.Indent(Doc.SoftLine, Node.Print(node.Condition)),
                            Doc.SoftLine
                        ),
                        Token.Print(node.CloseParenToken),
                        Doc.IfBreak(Doc.Null, Doc.SoftLine)
                    ),
                    OptionalBraces.Print(node.Statement, groupId)
                )
            );

            if (node.Else != null)
            {
                docs.Add(Doc.HardLine, Node.Print(node.Else));
            }

            return Doc.Concat(docs);
        }
    }
}
