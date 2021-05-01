using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ElseClause
    {
        public static Doc Print(ElseClauseSyntax node)
        {
            var docs = new List<Doc> { Token.Print(node.ElseKeyword) };
            var statement = Node.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                docs.Add(statement);
            }
            else if (node.Statement is IfStatementSyntax)
            {
                docs.Add(" ", statement);
            }
            else
            {
                // TODO 1 force braces here?
                docs.Add(Doc.Indent(Doc.HardLine, statement));
            }

            return Doc.Concat(docs);
        }
    }
}
