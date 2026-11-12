using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class MemberExpression
{
    public static Doc Print(MemberExpressionAst node, PrintContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            GetOperator(node),
            Node.Print(node.Member, context)
        );
    }

    internal static string GetOperator(MemberExpressionAst node)
    {
        return node.Static ? "::"
            : node.NullConditional ? "?."
            : ".";
    }
}
