using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ForEachStatement
{
    internal static Doc Print(ForEachStatementAst node, PrintContext context)
    {
        var docs = new List<Doc> { Statements.PrintLabel(node), "foreach " };
        if ((node.Flags & ForEachFlags.Parallel) != 0)
        {
            docs.Add("-parallel ");
        }

        if (node.ThrottleLimit is not null)
        {
            docs.Add("-throttlelimit ");
            docs.Add(Node.Print(node.ThrottleLimit, context));
            docs.Add(" ");
        }

        docs.Add("(");
        docs.Add(Node.Print(node.Variable, context));
        docs.Add(" in ");
        docs.Add(Node.Print(node.Condition, context));
        docs.Add(") ");
        docs.Add(StatementBlock.Print(node.Body, context, node.Condition));
        return Doc.Concat(docs);
    }
}
