using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TypeDefinition
{
    // The name is the token after the class, enum, or interface keyword.
    private static string GetName(TypeDefinitionAst node, PrintContext context)
    {
        var startOffset = node.Extent.StartOffset;
        foreach (var attribute in node.Attributes)
        {
            startOffset = Math.Max(startOffset, attribute.Extent.EndOffset);
        }

        var tokens = context.GetTokens(startOffset, node.Extent.EndOffset);
        return tokens.Count > 1 ? tokens[1].Text : node.Name;
    }

    internal static Doc Print(TypeDefinitionAst node, PrintContext context)
    {
        var docs = new List<Doc>();
        foreach (var attribute in node.Attributes)
        {
            docs.Add(context.PrintLeadingComments(attribute));
            docs.Add(Node.Print(attribute, context));
            docs.Add(context.PrintTrailingComments(attribute));
            docs.Add(Doc.HardLine);
        }

        docs.Add(
            node.IsEnum ? "enum "
            : node.IsInterface ? "interface "
            : "class "
        );
        docs.Add(GetName(node, context));

        if (node.BaseTypes.Count > 0)
        {
            docs.Add(" : ");
            docs.Add(Doc.Join(", ", node.BaseTypes.Select(o => Node.Print(o, context))));
        }

        var lines = new LineList();
        Ast? previous = null;
        foreach (var member in node.Members)
        {
            lines.AddWithComments(
                member,
                Node.Print(member, context),
                context,
                forceBlankLine: previous is FunctionMemberAst || member is FunctionMemberAst
            );
            previous = member;
        }

        lines.AddComments(context.GetDanglingComments(node), context);

        if (lines.IsEmpty)
        {
            docs.Add(" { }");
        }
        else
        {
            docs.Add(" {");
            docs.Add(Doc.Indent(Doc.HardLine, lines.ToDoc()));
            docs.Add(Doc.HardLine);
            docs.Add("}");
        }

        return Doc.Concat(docs);
    }
}
