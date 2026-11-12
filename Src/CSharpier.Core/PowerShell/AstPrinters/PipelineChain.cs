using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class PipelineChain
{
    public static Doc Print(PipelineChainAst node, PrintContext context)
    {
        var chain = new Stack<PipelineChainAst>();
        var current = node;
        while (true)
        {
            chain.Push(current);
            if (current.LhsPipelineChain is PipelineChainAst { Background: false } left)
            {
                current = left;
            }
            else
            {
                break;
            }
        }

        var first = Node.Print(chain.Peek().LhsPipelineChain, context);
        var forceBreak = false;
        var rest = new List<Doc>();
        foreach (var link in chain)
        {
            if (
                link.LhsPipelineChain.Extent.EndLineNumber
                != link.RhsPipeline.Extent.StartLineNumber
            )
            {
                forceBreak = true;
            }

            rest.Add(link.Operator == TokenKind.AndAnd ? " &&" : " ||");
            rest.Add(context.PrintTrailingComments(link.LhsPipelineChain));
            rest.Add(Doc.Line);
            rest.Add(context.PrintLeadingComments(link.RhsPipeline));
            rest.Add(Node.Print(link.RhsPipeline, context));
        }

        var result = Doc.Group(first, Doc.Indent(rest), forceBreak ? Doc.BreakParent : Doc.Null);
        return node.Background ? Doc.Concat(result, " &") : result;
    }
}
