using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

internal static class PowerShellPrinter
{
    internal static Doc Print(
        ScriptBlockAst scriptBlock,
        IReadOnlyList<IScriptExtent> comments
    )
    {
        var context = new PrintContext(comments);
        return Node.Print(scriptBlock, context);
    }
}
