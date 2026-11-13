using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class BlockStatement
{
    internal static Doc Print(BlockStatementAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
