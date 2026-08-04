using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class StatementBlock
{
    internal static Doc Print(StatementBlockAst node, PrintContext context)
    {
        if (node.Statements.Count == 0)
        {
            return "{ }";
        }

        return Doc.Concat(
            "{",
            Doc.Indent(
                Doc.HardLine,
                Statements.Print(
                    node.Statements,
                    context,
                    node.Extent.StartOffset,
                    node.Extent.EndOffset
                )
            ),
            Doc.HardLine,
            "}"
        );
    }
}
