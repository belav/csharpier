using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForStatementSyntax(ForStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var innerGroup = new List<Doc> { Doc.SoftLine };
            if (node.Declaration != null)
            {
                innerGroup.Add(
                    this.PrintVariableDeclarationSyntax(node.Declaration)
                );
            }
            innerGroup.Add(
                SeparatedSyntaxList.Print(node.Initializers, this.Print, " ")
            );
            innerGroup.Add(Token.Print(node.FirstSemicolonToken));
            if (node.Condition != null)
            {
                innerGroup.Add(Doc.Line, this.Print(node.Condition));
            }
            else
            {
                innerGroup.Add(Doc.SoftLine);
            }

            innerGroup.Add(Token.Print(node.SecondSemicolonToken));
            if (node.Incrementors.Any())
            {
                innerGroup.Add(Doc.Line);
            }
            else
            {
                innerGroup.Add(Doc.SoftLine);
            }
            innerGroup.Add(
                Doc.Indent(
                    SeparatedSyntaxList.Print(
                        node.Incrementors,
                        this.Print,
                        Doc.Line
                    )
                )
            );

            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                Token.PrintLeadingTrivia(node.ForKeyword),
                Doc.Group(
                    Token.PrintWithoutLeadingTrivia(node.ForKeyword),
                    " ",
                    Token.Print(node.OpenParenToken),
                    Doc.GroupWithId(
                        groupId,
                        Doc.Indent(innerGroup),
                        Doc.SoftLine
                    ),
                    Token.Print(node.CloseParenToken),
                    Doc.IfBreak(Doc.Null, Doc.SoftLine)
                )
            };

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
                // TODO 1 force braces? we do in if and else
                docs.Add(Doc.Indent(Doc.HardLine, this.Print(node.Statement)));
            }

            return Doc.Concat(docs);
        }
    }
}
