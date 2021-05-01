using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
                Token.Print(node.OpenParenToken),
                this.Print(node.Expression),
                Token.Print(node.CloseParenToken)
            };
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                docs.Add(statement);
            }
            else
            {
                docs.Add(Doc.Indent(Doc.HardLine, statement));
            }

            return Doc.Concat(docs);
        }
    }
}
