using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class BinaryExpression
{
    public static Doc Print(BinaryExpressionAst node, PrintContext context)
    {
        if (node.Operator == TokenKind.DotDot)
        {
            return Doc.Concat(
                Node.Print(node.Left, context),
                "..",
                Node.Print(node.Right, context)
            );
        }

        var chain = new Stack<BinaryExpressionAst>();
        var current = node;
        while (true)
        {
            chain.Push(current);
            if (
                current.Left is BinaryExpressionAst left
                && left.Operator != TokenKind.DotDot
                && GetPrecedence(left.Operator) == GetPrecedence(node.Operator)
            )
            {
                current = left;
            }
            else
            {
                break;
            }
        }

        var firstOperand = chain.Peek().Left;
        var first = Node.Print(firstOperand, context);

        // After a first operand like @( ) or { } the operator and the next operand stay on the
        // line of its closing delimiter. The same goes for ( ) when one short operand follows.
        var hugFirstOperand =
            firstOperand is ParenExpressionAst
                ? chain.Count == 1 && IsShort(node.Right)
                : Layout.IsDelimitedBlock(firstOperand);

        var rest = new List<Doc>();
        foreach (var link in chain)
        {
            rest.Add(" ");
            rest.Add(link.ErrorPosition.Text.ToLowerInvariant());
            var trailingComments = context.PrintTrailingComments(link.Left);
            rest.Add(trailingComments);
            if (trailingComments != Doc.Null)
            {
                rest.Add(Doc.Line);
            }
            else if (link.Operator == TokenKind.Format || Layout.IsMultiLineString(link.Left))
            {
                rest.Add(" ");
            }
            else if (link.Left == firstOperand && hugFirstOperand)
            {
                rest.Add(" ");
            }
            else
            {
                rest.Add(Doc.Line);
            }

            rest.Add(context.PrintLeadingComments(link.Right));
            rest.Add(Node.Print(link.Right, context));
        }

        if (hugFirstOperand)
        {
            return Doc.Concat(first, Doc.Group(Doc.Indent(rest)));
        }

        return Layout.IsSoleContentOfParens(node)
            ? Doc.Group(first, Doc.Concat(rest))
            : Doc.Group(first, Doc.Indent(rest));
    }

    private static bool IsShort(ExpressionAst node)
    {
        return node
                is VariableExpressionAst
                    or ConstantExpressionAst
                    or TypeExpressionAst
                    or MemberExpressionAst { Expression: VariableExpressionAst }
                    or IndexExpressionAst { Target: VariableExpressionAst }
            || (node is ExpandableStringExpressionAst && !Layout.IsMultiLine(node));
    }

    private static TokenFlags GetPrecedence(TokenKind kind)
    {
        return kind.GetTraits() & TokenFlags.BinaryPrecedenceMask;
    }
}
