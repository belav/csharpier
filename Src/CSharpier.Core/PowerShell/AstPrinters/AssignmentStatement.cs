using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class AssignmentStatement
{
    public static Doc Print(AssignmentStatementAst node, PrintContext context)
    {
        // A right side that cannot break on its own moves to the next line when it does not fit.
        var breakAfterOperator =
            node.Right is CommandExpressionAst { Redirections.Count: 0 } commandExpression
            && Layout.HasNoBreakPoint(commandExpression.Expression)
            && !context.HasCommentIn(node.Right.Extent);

        var left = Doc.Concat(Node.Print(node.Left, context), " ", node.ErrorPosition.Text);
        var right = Node.Print(node.Right, context);

        if (breakAfterOperator)
        {
            return Doc.Group(left, Doc.Indent(Doc.Line, right));
        }

        return Doc.Concat(left, " ", right);
    }
}
