using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell;

internal static class Verbatim
{
    // Emits the exact source text of an extent. Single-line text becomes a plain string; multi-line
    // text keeps its interior lines verbatim (via LiteralLine, which does not add indentation) so
    // here-strings and multi-line pipelines round-trip unchanged. Comments inside the extent are
    // part of the text, so they count as printed.
    internal static Doc Print(IScriptExtent extent, PrintContext context)
    {
        return Print(extent.StartOffset, extent.EndOffset, context);
    }

    // Prints a node from its source text. The & of a background pipeline lies outside the extent
    // of the pipeline and of an assignment that ends with it, so it is added back.
    internal static Doc PrintNode(Ast node, PrintContext context)
    {
        var doc = Print(node.Extent, context);
        return EndsWithBackgroundOperator(node) ? Doc.Concat(doc, " &") : doc;
    }

    private static bool EndsWithBackgroundOperator(Ast node)
    {
        while (true)
        {
            switch (node)
            {
                case PipelineAst { Background: true }:
                case PipelineChainAst { Background: true }:
                    return true;
                case AssignmentStatementAst assignment:
                    node = assignment.Right;
                    break;
                default:
                    return false;
            }
        }
    }

    internal static Doc Print(int startOffset, int endOffset, PrintContext context)
    {
        context.MarkCommentsPrintedIn(startOffset, endOffset);

        var text = context.GetText(startOffset, endOffset);
        if (!text.Contains('\n'))
        {
            return text;
        }

        var lines = text.Replace("\r\n", "\n").Split('\n');
        var docs = new List<Doc> { lines[0] };
        for (var i = 1; i < lines.Length; i++)
        {
            docs.Add(Doc.LiteralLine);
            docs.Add(lines[i]);
        }

        return Doc.Concat(docs);
    }
}
