using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class DynamicKeywordStatement
{
    internal static Doc Print(DynamicKeywordStatementAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
