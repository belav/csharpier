using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ParamBlock
{
    internal static Doc Print(ParamBlockAst node, PrintContext context)
    {
        var docs = new List<Doc>();

        // The attributes of a param block, such as [CmdletBinding()], sit in front of its extent.
        foreach (var attribute in node.Attributes)
        {
            docs.Add(context.PrintLeadingComments(attribute));
            docs.Add(Node.Print(attribute, context));
            docs.Add(context.PrintTrailingComments(attribute));
            docs.Add(Doc.HardLine);
        }

        var parameters = node.Parameters;
        var danglingComments = context.GetDanglingComments(node);
        if (parameters.Count == 0 && danglingComments.Count == 0)
        {
            docs.Add("param()");
            return Doc.Concat(docs);
        }

        var blankLineBetween = parameters.Any(Parameter.HasAttribute);
        var expanded = Layout.IsMultiLine(node) || context.HasCommentIn(node.Extent);

        if (!expanded)
        {
            var contents = new List<Doc> { Doc.SoftLine };
            for (var index = 0; index < parameters.Count; index++)
            {
                if (index > 0)
                {
                    contents.Add(",");
                    contents.Add(Doc.Line);
                    if (blankLineBetween)
                    {
                        contents.Add(Doc.SoftLine);
                    }
                }

                contents.Add(Parameter.Print(parameters[index], context, false));
            }

            docs.Add(Doc.Group("param(", Doc.Indent(contents), Doc.SoftLine, ")"));
            return Doc.Concat(docs);
        }

        var lines = new LineList();
        for (var index = 0; index < parameters.Count; index++)
        {
            var parameter = parameters[index];
            lines.Add(
                Doc.Concat(
                    context.PrintLeadingComments(parameter),
                    PrintExpandedParameter(parameter, context),
                    index < parameters.Count - 1 ? "," : Doc.Null,
                    context.PrintTrailingComments(parameter)
                ),
                context.GetStartLine(parameter),
                context.GetEndLine(parameter),
                blankLineBetween
            );
        }

        lines.AddComments(danglingComments, context);

        // Node.Print would only reprint the extent, which leaves out the attributes.
        if (context.HasUnprintedCommentIn(node.Extent))
        {
            return Verbatim.Print(
                PrintContext.GetOwnStartOffset(node),
                node.Extent.EndOffset,
                context
            );
        }

        docs.Add("param(");
        docs.Add(Doc.Indent(Doc.HardLine, lines.ToDoc()));
        docs.Add(Doc.HardLine);
        docs.Add(")");
        return Doc.Concat(docs);
    }

    private static Doc PrintExpandedParameter(ParameterAst parameter, PrintContext context)
    {
        var doc = Parameter.Print(parameter, context, true);
        if (context.HasUnprintedCommentIn(parameter.Extent))
        {
            doc = Verbatim.Print(parameter.Extent, context);
        }

        return doc;
    }
}
