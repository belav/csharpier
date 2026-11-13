using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

internal static class PowerShellPrinter
{
    internal static Doc Print(ScriptBlockAst scriptBlock, Token[] tokens, string source)
    {
        var context = new PrintContext(scriptBlock, tokens, source);
        return Node.Print(scriptBlock, context);
    }
}
