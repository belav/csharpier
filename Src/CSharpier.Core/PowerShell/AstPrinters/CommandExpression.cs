using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class CommandExpression
{
    public static Doc Print(CommandExpressionAst node, PrintContext context)
    {
        var expression = Node.Print(node.Expression, context);
        if (node.Redirections.Count == 0)
        {
            return expression;
        }

        var docs = new List<Doc> { expression };
        foreach (var redirection in node.Redirections)
        {
            docs.Add(" ");
            docs.Add(Node.Print(redirection, context));
        }

        return Doc.Concat(docs);
    }
}
