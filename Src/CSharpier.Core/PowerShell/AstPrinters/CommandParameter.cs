using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class CommandParameter
{
    public static Doc Print(CommandParameterAst node, PrintContext context)
    {
        if (node.Argument is null)
        {
            return Verbatim.Print(node.Extent, context);
        }

        var name = context.GetTokenText(node.Extent.StartOffset, node.Argument.Extent.StartOffset);

        return Doc.Concat(name, Node.Print(node.Argument, context));
    }
}
