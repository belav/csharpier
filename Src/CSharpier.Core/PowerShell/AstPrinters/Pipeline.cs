using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Pipeline
{
    public static Doc Print(PipelineAst node, PrintContext context)
    {
        var elements = node.PipelineElements;
        var first = Node.Print(elements[0], context);
        var result = first;

        if (elements.Count > 1)
        {
            var forceBreak = false;
            var hasMultiLineElement = false;
            var rest = new List<Doc>();
            for (var index = 1; index < elements.Count; index++)
            {
                var previous = elements[index - 1];
                var current = elements[index];
                if (previous.Extent.EndLineNumber != current.Extent.StartLineNumber)
                {
                    forceBreak = true;
                }

                if (Layout.IsMultiLine(previous) || Layout.IsMultiLine(current))
                {
                    hasMultiLineElement = true;
                }

                rest.Add(Command.IsStopParsing(previous) ? "|" : " |");
                rest.Add(context.PrintTrailingComments(previous));
                rest.Add(Doc.Line);
                rest.Add(context.PrintLeadingComments(current));
                rest.Add(Node.Print(current, context));
            }

            if (!forceBreak && hasMultiLineElement && !context.HasCommentIn(node.Extent))
            {
                var docs = new List<Doc> { first };
                for (var index = 1; index < elements.Count; index++)
                {
                    docs.Add(Command.IsStopParsing(elements[index - 1]) ? "| " : " | ");
                    docs.Add(rest[(index * 5) - 1]);
                }

                return node.Background ? Doc.Concat(Doc.Concat(docs), " &") : Doc.Concat(docs);
            }

            result = Doc.Group(first, Doc.Indent(rest), forceBreak ? Doc.BreakParent : Doc.Null);
        }

        return node.Background ? Doc.Concat(result, " &") : result;
    }
}
