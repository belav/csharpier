using System.Collections.Generic;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintElseClauseSyntax(ElseClauseSyntax node)
        {
            var docs = new List<Doc> { SyntaxTokens.Print(node.ElseKeyword) };
            var statement = this.Print(node.Statement);
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
                docs.Add(Docs.Indent(Docs.HardLine, statement));
            }

            return Docs.Concat(docs);
        }
    }
}
