using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class DoWhileStatement
{
    internal static Doc Print(DoWhileStatementAst node, PrintContext context)
    {
        return Print(node, "while", context);
    }

    internal static Doc Print(LoopStatementAst node, string keyword, PrintContext context)
    {
        return Doc.Concat(
            Statements.PrintLabel(node),
            "do ",
            StatementBlock.Print(node.Body, context),
            StatementBlock.PrintKeywordAfter(node.Body, node.Condition, keyword, context),
            " ",
            Condition.Print(node.Condition, context)
        );
    }
}
