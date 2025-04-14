using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class QueryBody
{
    public static Doc Print(QueryBodySyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null, null]);
        docs.Append(Doc.Join(Doc.Line, node.Clauses.Select(o => Node.Print(o, context))));

        if (node.Clauses.Count > 0)
        {
            docs.Append(Doc.Line);
        }

        docs.Append(Node.Print(node.SelectOrGroup, context));
        if (node.Continuation != null)
        {
            docs.Append(" ", QueryContinuation.Print(node.Continuation, context));
        }

        return Doc.Concat(ref docs);
    }
}
