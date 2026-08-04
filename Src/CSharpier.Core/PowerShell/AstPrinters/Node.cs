using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Node
{
    internal static Doc Print(Ast node, PrintContext context)
    {
        // Never reformat something that contains a comment - emit it verbatim so the comment survives.
        if (node is StatementAst && context.HasCommentIn(node.Extent))
        {
            return Verbatim.Print(node.Extent);
        }

        return node switch
        {
            ScriptBlockAst scriptBlock => ScriptBlock.Print(scriptBlock, context),
            IfStatementAst ifStatement => IfStatement.Print(ifStatement, context),
            WhileStatementAst whileStatement => WhileStatement.Print(whileStatement, context),
            ForEachStatementAst forEach => ForEachStatement.Print(forEach, context),
            FunctionDefinitionAst function => FunctionDefinition.Print(function, context),
            _ => Verbatim.Print(node.Extent),
        };
    }
}
