using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TryStatement
{
    internal static Doc Print(TryStatementAst node, PrintContext context)
    {
        var docs = new List<Doc> { "try ", StatementBlock.Print(node.Body, context) };

        // A comment after a catch block on its line is a trailing comment of the catch clause.
        Ast previous = node.Body;
        foreach (var catchClause in node.CatchClauses)
        {
            docs.Add(StatementBlock.PrintSeparatorAfter(previous, catchClause, context));
            docs.Add(Node.Print(catchClause, context));
            previous = catchClause;
        }

        if (node.Finally is not null)
        {
            docs.Add(StatementBlock.PrintKeywordAfter(previous, node.Finally, "finally", context));
            docs.Add(" ");
            docs.Add(StatementBlock.Print(node.Finally, context));
        }

        return Doc.Concat(docs);
    }
}
