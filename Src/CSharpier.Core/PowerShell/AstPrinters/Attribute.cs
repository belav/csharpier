using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Attribute
{
    internal static Doc Print(AttributeAst node, PrintContext context)
    {
        var arguments = new List<Ast>(node.PositionalArguments.Count + node.NamedArguments.Count);
        arguments.AddRange(node.PositionalArguments);
        arguments.AddRange(node.NamedArguments);
        if (arguments.Count == 0)
        {
            return Doc.Concat("[", node.TypeName.Extent.Text, "()]");
        }

        arguments.Sort(
            (first, second) => first.Extent.StartOffset.CompareTo(second.Extent.StartOffset)
        );

        return Doc.Concat(
            "[",
            node.TypeName.Extent.Text,
            ArgumentList.Print("(", arguments, ")", context),
            "]"
        );
    }
}
