using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class LockStatement
    {
        public static Doc Print(LockStatementSyntax node)
        {
            var docs = new List<Doc>
            {
                Token.Print(node.LockKeyword, " "),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Expression),
                Token.Print(node.CloseParenToken)
            };
            var statement = Node.Print(node.Statement);
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
