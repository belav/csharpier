using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class WhileStatement
{
    internal static Doc Print(WhileStatementAst node, PrintContext context)
    {
        return Doc.Concat(
            Statements.PrintLabel(node),
            "while ",
            Condition.Print(node.Condition, context),
            " ",
            StatementBlock.Print(node.Body, context, node.Condition)
        );
    }
}
