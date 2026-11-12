using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class PropertyMember
{
    internal static Doc Print(PropertyMemberAst node, PrintContext context)
    {
        var isEnumMember = node.Parent is TypeDefinitionAst { IsEnum: true };

        var docs = new List<Doc>();
        foreach (var attribute in node.Attributes)
        {
            docs.Add(context.PrintLeadingComments(attribute));
            docs.Add(Node.Print(attribute, context));
            docs.Add(context.PrintTrailingComments(attribute));
            docs.Add(Doc.HardLine);
        }

        if (!isEnumMember)
        {
            if (node.IsHidden)
            {
                docs.Add("hidden ");
            }

            if (node.IsStatic)
            {
                docs.Add("static ");
            }

            if (node.PropertyType is not null)
            {
                docs.Add(Node.Print(node.PropertyType, context));
            }
        }

        docs.Add(isEnumMember ? node.Name : GetVariable(node, context));
        if (node.InitialValue is not null)
        {
            docs.Add(" = ");
            docs.Add(Node.Print(node.InitialValue, context));
        }

        return Doc.Concat(docs);
    }

    // A name such as ${config-file} needs its braces, and the tree only has the name.
    private static string GetVariable(PropertyMemberAst node, PrintContext context)
    {
        var startOffset = node.Extent.StartOffset;
        foreach (var attribute in node.Attributes)
        {
            startOffset = Math.Max(startOffset, attribute.Extent.EndOffset);
        }

        if (node.PropertyType is not null)
        {
            startOffset = Math.Max(startOffset, node.PropertyType.Extent.EndOffset);
        }

        var text = context.GetText(startOffset, node.Extent.EndOffset);
        var start = text.IndexOf("${", StringComparison.Ordinal);
        var end = start < 0 ? -1 : text.IndexOf('}', start);
        return end < 0 || text[..start].Contains('$') ? "$" + node.Name : text[start..(end + 1)];
    }
}
