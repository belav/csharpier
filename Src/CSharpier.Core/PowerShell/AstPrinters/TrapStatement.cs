using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TrapStatement
{
    internal static Doc Print(TrapStatementAst node, PrintContext context)
    {
        return Doc.Concat(
            "trap ",
            node.TrapType is null ? Doc.Null : Doc.Concat(Node.Print(node.TrapType, context), " "),
            StatementBlock.Print(node.Body, context, node.TrapType)
        );
    }
}
