using System.Collections.Generic;
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

            var groupId = GroupIdGenerator.GenerateGroupIdFor(node);

            docs.Add(
                this.PrintSyntaxToken(
                    node.IfKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    Docs.Indent(Docs.SoftLine, this.Print(node.Condition)),
                    Docs.SoftLine
                ),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                docs.Add(statement);
            }
            else
            {
                // TODO 1 force braces here? make an option?
                docs.Add(Docs.Indent(Docs.Concat(Docs.HardLine, statement)));
            }

            if (node.Else != null)
            {
                docs.Add(Docs.HardLine, this.Print(node.Else));
            }

            GroupIdGenerator.RemoveGroupIdFor(node);

            return Docs.Concat(docs);
        }
    }
}
