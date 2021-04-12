using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLockStatementSyntax(LockStatementSyntax node)
        {
            var docs = new List<Doc>
            {
                this.PrintSyntaxToken(
                    node.LockKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Expression),
                this.PrintSyntaxToken(node.CloseParenToken)
            };
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                docs.Add(statement);
            }
            else
            {
                docs.Add(Docs.Indent(Docs.HardLine, statement));
            }

            return Docs.Concat(docs);
        }
    }
}
