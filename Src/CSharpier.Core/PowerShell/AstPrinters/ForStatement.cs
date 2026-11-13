using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ForStatement
{
    internal static Doc Print(ForStatementAst node, PrintContext context)
    {
        var docs = new List<Doc> { Statements.PrintLabel(node), "for (" };
        if (node.Initializer is not null)
        {
            docs.Add(Node.Print(node.Initializer, context));
        }

        docs.Add(";");
        if (node.Condition is not null)
        {
            docs.Add(" ");
            docs.Add(Node.Print(node.Condition, context));
        }

        docs.Add(";");
        if (node.Iterator is not null)
        {
            docs.Add(" ");
            docs.Add(Node.Print(node.Iterator, context));
        }

        docs.Add(") ");
        docs.Add(StatementBlock.Print(node.Body, context));
        return Doc.Concat(docs);
    }
}
