using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class MergingRedirection
{
    public static Doc Print(MergingRedirectionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
