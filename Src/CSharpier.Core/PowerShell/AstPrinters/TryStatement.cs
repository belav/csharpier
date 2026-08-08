using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TryStatement
{
    internal static Doc Print(TryStatementAst node, PrintContext context)
    {
        return Doc.Concat(
            "try ",
            StatementBlock.Print(node.Body, context),
            Doc.Join(
                Doc.Null,
                node.CatchClauses.Select(o => Doc.Concat(" catch ", PrintCatchClause(o, context)))
            ),
            node.Finally != null
                ? Doc.Concat(" finally ", StatementBlock.Print(node.Finally, context))
                : Doc.Null
        );
    }

    private static Doc PrintCatchClause(CatchClauseAst node, PrintContext context)
    {
        // TODO 1894 what about catching specific things?
        return StatementBlock.Print(node.Body, context);
    }
}
