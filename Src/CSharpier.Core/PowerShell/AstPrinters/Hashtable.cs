using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Hashtable
{
    public static Doc Print(HashtableAst node, PrintContext context)
    {
        var pairs = node.KeyValuePairs;
        var danglingComments = context.GetDanglingComments(node);
        if (pairs.Count == 0 && danglingComments.Count == 0)
        {
            return "@{}";
        }

        var expanded =
            danglingComments.Count > 0
            || pairs[0].Item1.Extent.StartLineNumber != node.Extent.StartLineNumber;
        foreach (var (key, value) in pairs)
        {
            if (
                context.GetLeadingComments(key).Count > 0
                || context.GetTrailingComments(value).Count > 0
            )
            {
                expanded = true;
            }
        }

        if (!expanded)
        {
            var flat = new List<Doc> { Doc.Line };
            for (var index = 0; index < pairs.Count; index++)
            {
                if (index > 0)
                {
                    flat.Add(Doc.IfBreak(Doc.Null, ";"));
                    flat.Add(Doc.Line);
                }

                flat.Add(PrintPair(pairs[index], context));
            }

            return Doc.Group("@{", Doc.Indent(flat), Doc.Line, "}");
        }

        var docs = new List<Doc>();
        int? previousEndLine = null;

        void Add(Doc doc, int startLine, int endLine)
        {
            docs.Add(Doc.HardLine);
            if (previousEndLine is not null && startLine - previousEndLine > 1)
            {
                docs.Add(Doc.HardLine);
            }

            docs.Add(doc);
            previousEndLine = endLine;
        }

        foreach (var pair in pairs)
        {
            Add(
                Doc.Concat(
                    context.PrintLeadingComments(pair.Item1),
                    PrintPair(pair, context),
                    context.PrintTrailingComments(pair.Item2)
                ),
                context.GetStartLine(pair.Item1),
                context.GetEndLine(pair.Item2)
            );
        }

        foreach (var comment in danglingComments)
        {
            Add(context.PrintComment(comment), comment.StartLineNumber, comment.EndLineNumber);
        }

        return Doc.Concat("@{", Doc.Indent(docs), Doc.HardLine, "}");
    }

    private static Doc PrintPair(Tuple<ExpressionAst, StatementAst> pair, PrintContext context)
    {
        return Doc.Concat(Node.Print(pair.Item1, context), " = ", Node.Print(pair.Item2, context));
    }
}
