using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ReturnStatement
{
    internal static Doc Print(ReturnStatementAst node, PrintContext context)
    {
        return node.Pipeline is null
            ? "return"
            : Doc.Concat("return ", Node.Print(node.Pipeline, context));
    }
}
