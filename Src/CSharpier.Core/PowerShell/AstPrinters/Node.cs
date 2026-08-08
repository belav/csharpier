using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Node
{
    internal static Doc Print(Ast node, PrintContext context)
    {
        return node switch
        {
            ExitStatementAst exitStatement => ExitStatement.Print(exitStatement, context),
            ForEachStatementAst forEach => ForEachStatement.Print(forEach, context),
            FunctionDefinitionAst function => FunctionDefinition.Print(function, context),
            IfStatementAst ifStatement => IfStatement.Print(ifStatement, context),
            ScriptBlockAst scriptBlock => ScriptBlock.Print(scriptBlock, context),
            TryStatementAst tryStatement => TryStatement.Print(tryStatement, context),
            WhileStatementAst whileStatement => WhileStatement.Print(whileStatement, context),
            _ => node.GetType().ToString(),
        };
    }
}
