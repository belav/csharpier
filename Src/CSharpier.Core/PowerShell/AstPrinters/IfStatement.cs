using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class IfStatement
{
    internal static Doc Print(IfStatementAst node, PrintContext context)
    {
        var docs = new List<Doc>();

        for (var i = 0; i < node.Clauses.Count; i++)
        {
            var (condition, body) = (node.Clauses[i].Item1, node.Clauses[i].Item2);
            docs.Add(i == 0 ? "if (" : " elseif (");
            docs.Add(Verbatim.Print(condition.Extent));
            docs.Add(") ");
            docs.Add(StatementBlock.Print(body, context));
        }

        if (node.ElseClause is not null)
        {
            docs.Add(" else ");
            docs.Add(StatementBlock.Print(node.ElseClause, context));
        }

        return Doc.Concat(docs);
    }
}
