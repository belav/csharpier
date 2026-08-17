using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Command
{
    public static Doc Print(CommandAst node, PrintContext context)
    {
        return Doc.Join(" ", node.CommandElements.Select(o => Node.Print(o, context)));
    }
}
