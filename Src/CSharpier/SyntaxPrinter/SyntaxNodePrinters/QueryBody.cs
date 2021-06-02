using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class QueryBody
    {
        public static Doc Print(QueryBodySyntax node)
        {
            var docs = new List<Doc> { Doc.Join(Doc.Line, node.Clauses.Select(Node.Print)) };
            if (node.Clauses.Count > 0)
            {
                docs.Add(Doc.Line);
            }

            docs.Add(Node.Print(node.SelectOrGroup));
            if (node.Continuation != null)
            {
                docs.Add(" ", QueryContinuation.Print(node.Continuation));
            }

            return Doc.Concat(docs);
        }
    }
}
