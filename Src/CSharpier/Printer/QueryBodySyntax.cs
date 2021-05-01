using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryBodySyntax(QueryBodySyntax node)
        {
            var docs = new List<Doc>
            {
                Doc.Join(Doc.Line, node.Clauses.Select(this.Print))
            };
            if (node.Clauses.Count > 0)
            {
                docs.Add(Doc.Line);
            }

            docs.Add(this.Print(node.SelectOrGroup));
            if (node.Continuation != null)
            {
                docs.Add(
                    " ",
                    this.PrintQueryContinuationSyntax(node.Continuation)
                );
            }

            return Doc.Concat(docs);
        }
    }
}
