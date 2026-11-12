using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Parameter
{
    internal static Doc Print(ParameterAst node, PrintContext context)
    {
        return Print(node, context, false);
    }

    // Attributes go on their own lines above the parameter when the parameter list is broken. The
    // type constraints right in front of the name stay attached to it.
    internal static Doc Print(ParameterAst node, PrintContext context, bool expanded)
    {
        var attributes = node.Attributes;
        var firstInline = attributes.Count;
        while (firstInline > 0 && attributes[firstInline - 1] is TypeConstraintAst)
        {
            firstInline--;
        }

        var line = expanded ? (Doc)Doc.HardLine : Doc.Line;
        var docs = new List<Doc>();
        var onNewLine = true;

        void AddLeadingComments(Ast child)
        {
            var leadingComments = context.PrintLeadingComments(child);
            if (leadingComments != Doc.Null && !onNewLine)
            {
                docs.Add(line);
            }

            docs.Add(leadingComments);
        }

        for (var index = 0; index < attributes.Count; index++)
        {
            var attribute = attributes[index];
            AddLeadingComments(attribute);
            docs.Add(Node.Print(attribute, context));
            var trailingComments = context.PrintTrailingComments(attribute);
            docs.Add(trailingComments);
            onNewLine = index < firstInline || trailingComments != Doc.Null;
            if (onNewLine)
            {
                docs.Add(line);
            }
        }

        AddLeadingComments(node.Name);
        docs.Add(Node.Print(node.Name, context));
        if (node.DefaultValue is not null)
        {
            docs.Add(" = ");
            docs.Add(Node.Print(node.DefaultValue, context));
        }

        return Doc.Concat(docs);
    }

    internal static bool HasAttribute(ParameterAst node)
    {
        return node.Attributes.Any(o => o is AttributeAst);
    }
}
