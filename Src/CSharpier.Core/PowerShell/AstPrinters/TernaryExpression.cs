using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TernaryExpression
{
    public static Doc Print(TernaryExpressionAst node, PrintContext context)
    {
        return Doc.Group(
            Node.Print(node.Condition, context),
            " ?",
            Doc.Indent(
                Doc.Line,
                Node.Print(node.IfTrue, context),
                " :",
                Doc.Line,
                Node.Print(node.IfFalse, context)
            )
        );
    }
}
