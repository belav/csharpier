using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ArrayLiteral
{
    public static Doc Print(ArrayLiteralAst node, PrintContext context)
    {
        var elements = node.Elements;
        if (elements.Count == 1)
        {
            return Doc.Concat(",", Node.Print(elements[0], context));
        }

        // A command line cannot wrap, so an array that is a command argument only breaks where
        // the source already had it on several lines. That includes the place after a trailing
        // block comment: a break there would put the elements on several lines, and the next
        // format would then break after every comma. A trailing line comment only comes before
        // an element on a later line, where the separator is already a hard line.
        Doc separator = Layout.IsInsideIndex(node) ? " " : Doc.Line;
        if (node.Parent is CommandAst or CommandParameterAst)
        {
            separator = " ";
            for (var index = 1; index < elements.Count; index++)
            {
                if (
                    elements[index - 1].Extent.EndLineNumber
                    != elements[index].Extent.StartLineNumber
                )
                {
                    separator = Doc.HardLine;
                }
            }
        }

        var first = Doc.Concat(
            context.PrintLeadingComments(elements[0]),
            Node.Print(elements[0], context)
        );
        var rest = new List<Doc>();
        for (var index = 1; index < elements.Count; index++)
        {
            rest.Add(",");
            rest.Add(context.PrintTrailingComments(elements[index - 1]));
            rest.Add(separator);
            rest.Add(context.PrintLeadingComments(elements[index]));
            rest.Add(Node.Print(elements[index], context));
        }

        rest.Add(context.PrintTrailingComments(elements[^1]));

        if (Layout.IsSoleContentOfParens(node))
        {
            return Doc.Concat(first, Doc.Concat(rest));
        }

        // The right side of a binary operator is already indented by the binary expression.
        return
            node.Parent is BinaryExpressionAst binaryExpression
            && binaryExpression.Right == node
            && Layout.CanBreakAfterOperator(binaryExpression)
            && !Layout.IsSoleContentOfParens(binaryExpression)
            ? Doc.Group(first, Doc.Concat(rest))
            : Doc.Group(first, Doc.Indent(rest));
    }
}
