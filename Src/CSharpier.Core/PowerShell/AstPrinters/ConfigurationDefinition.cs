using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ConfigurationDefinition
{
    internal static Doc Print(ConfigurationDefinitionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
