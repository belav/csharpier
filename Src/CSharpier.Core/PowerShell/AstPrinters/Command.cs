using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Command
{
    public static Doc Print(CommandAst node, PrintContext context)
    {
        foreach (var element in node.CommandElements)
        {
            if (element is StringConstantExpressionAst && element.Extent.Text == "--%")
            {
                return Verbatim.Print(node.Extent, context);
            }
        }

        var items = new List<Ast>(node.CommandElements.Count + node.Redirections.Count);
        items.AddRange(node.CommandElements);
        items.AddRange(node.Redirections);
        if (node.Redirections.Count > 0)
        {
            items.Sort(
                (first, second) => first.Extent.StartOffset.CompareTo(second.Extent.StartOffset)
            );
        }

        var invocationOperator = node.InvocationOperator switch
        {
            TokenKind.Ampersand => "& ",
            TokenKind.Dot => ". ",
            _ => string.Empty,
        };

        // Only the lines that follow a backtick continuation are indented.
        var docs = new List<Doc> { invocationOperator, PrintElement(items[0], context) };
        var rest = docs;
        for (var index = 1; index < items.Count; index++)
        {
            var previous = items[index - 1];
            var current = items[index];
            if (previous.Extent.EndLineNumber != current.Extent.StartLineNumber)
            {
                if (rest == docs)
                {
                    rest = [];
                }

                rest.Add(" `");
                rest.Add(Doc.HardLine);
            }
            else if (previous.Extent.EndOffset != current.Extent.StartOffset)
            {
                rest.Add(" ");
            }

            rest.Add(PrintElement(current, context));
        }

        if (rest != docs)
        {
            docs.Add(Doc.Indent(rest));
        }

        return Doc.Concat(docs);
    }

    // A block comment after an element on the same line stays there. A line comment cannot be
    // followed by more arguments, so it is left for the fallback.
    private static Doc PrintElement(Ast element, PrintContext context)
    {
        var doc = Node.Print(element, context);
        var trailingComments = context.GetTrailingComments(element);
        if (trailingComments.Count == 0 || trailingComments.Any(PrintContext.IsLineComment))
        {
            return doc;
        }

        return Doc.Concat(doc, context.PrintTrailingComments(element));
    }
}
