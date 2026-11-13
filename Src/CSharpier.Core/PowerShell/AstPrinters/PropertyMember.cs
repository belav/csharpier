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

        docs.Add(isEnumMember ? GetEnumMemberName(node, context) : GetVariable(node, context));
        if (node.InitialValue is not null)
        {
            docs.Add(" = ");
            docs.Add(Node.Print(node.InitialValue, context));
        }

        return Doc.Concat(docs);
    }

    private static string GetEnumMemberName(PropertyMemberAst node, PrintContext context)
    {
        var startOffset = node.Extent.StartOffset;
        foreach (var attribute in node.Attributes)
        {
            startOffset = Math.Max(startOffset, attribute.Extent.EndOffset);
        }

        var tokens = context.GetTokens(startOffset, node.Extent.EndOffset);
        return tokens.Count > 0 ? tokens[0].Text : node.Name;
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

        foreach (var token in context.GetTokens(startOffset, node.Extent.EndOffset))
        {
            if (token.Kind == TokenKind.Variable)
            {
                return token.Text;
            }
        }

        return "$" + node.Name;
    }
}
