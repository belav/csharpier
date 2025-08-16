using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

// this is loosely based on prettier/src/language-js/print/binaryish.js
internal static class BinaryExpression
{
    public static Doc Print(BinaryExpressionSyntax node, PrintingContext context)
    {
        var docs = PrintBinaryExpression(node, context);

        if (node.Parent is IfStatementSyntax)
        {
            // avoid grouping here so that the ifBreaks in IfStatement can understand when
            // this BinaryExpression breaks
            return Doc.Concat(docs);
        }

        var shouldNotIndent =
            node.Parent
                is ArrowExpressionClauseSyntax
                    or AssignmentExpressionSyntax
                    or CatchFilterClauseSyntax
                    or CheckedExpressionSyntax
                    or DoStatementSyntax
                    or EqualsValueClauseSyntax
                    or ParenthesizedExpressionSyntax
                    or ParenthesizedLambdaExpressionSyntax
                    or SimpleLambdaExpressionSyntax
                    or ReturnStatementSyntax
                    or SwitchExpressionSyntax
                    or SwitchStatementSyntax
                    or WhereClauseSyntax
                    or WhileStatementSyntax
            || (
                node.Parent is ConditionalExpressionSyntax conditionalExpressionSyntax
                && conditionalExpressionSyntax.WhenTrue != node
                && conditionalExpressionSyntax.WhenFalse != node
            );

        return shouldNotIndent
            ? Doc.Group(docs)
            : Doc.Group(docs[0], Doc.Indent(docs.Skip(1).ToList()));
    }

    // The goal of this is to group operators of the same precedence such that they all break or none of them break
    // for example the following should break on the && before it breaks on the !=
    /* (
        one != two
        && three != four
        && five != six
     */
    private static List<Doc> PrintBinaryExpression(SyntaxNode node, PrintingContext context)
    {
        if (node is not BinaryExpressionSyntax binaryExpressionSyntax)
        {
            return [Doc.Group(Node.Print(node, context))];
        }

        if (context.State.PrintingDepth > 200)
        {
            throw new InTooDeepException();
        }

        context.State.PrintingDepth++;
        try
        {
            var docs = new List<Doc>();

            // This group ensures that something like == 0 does not end up on its own line
            var shouldGroup =
                binaryExpressionSyntax.Kind() != binaryExpressionSyntax.Parent!.Kind()
                && GetPrecedence(binaryExpressionSyntax)
                    != GetPrecedence(binaryExpressionSyntax.Parent!)
                && binaryExpressionSyntax.Left.GetType() != binaryExpressionSyntax.GetType()
                && binaryExpressionSyntax.Right.GetType() != binaryExpressionSyntax.GetType()
                && binaryExpressionSyntax.Left is not IsPatternExpressionSyntax;

            // nested ?? have the next level on the right side, everything else has it on the left
            var binaryOnTheRight = binaryExpressionSyntax.Kind() == SyntaxKind.CoalesceExpression;
            if (binaryOnTheRight)
            {
                var chain = 0;
                var possibleInvocation = binaryExpressionSyntax.Left;
                while (possibleInvocation is not null)
                {
                    if (possibleInvocation is InvocationExpressionSyntax invocationExpression)
                    {
                        possibleInvocation = invocationExpression.Expression;
                        chain++;
                    }
                    else if (
                        possibleInvocation
                        is MemberAccessExpressionSyntax memberAccessExpressionSyntax
                    )
                    {
                        possibleInvocation = memberAccessExpressionSyntax.Expression;
                        chain++;
                    }
                    else if (
                        possibleInvocation
                        is ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax
                    )
                    {
                        possibleInvocation = conditionalAccessExpressionSyntax.Expression;
                        chain++;
                    }
                    else if (
                        possibleInvocation
                        is ElementAccessExpressionSyntax elementAccessExpressionSyntax
                    )
                    {
                        possibleInvocation = elementAccessExpressionSyntax.Expression;
                        chain++;
                    }
                    else
                    {
                        possibleInvocation = null;
                    }
                }
                var leftDoc = Node.Print(binaryExpressionSyntax.Left, context);
                if (chain > 3)
                {
                    docs.Add(
                        Doc.Group(leftDoc, Doc.Line),
                        Token.Print(binaryExpressionSyntax.OperatorToken, context),
                        " "
                    );
                }
                else
                {
                    docs.Add(
                        leftDoc,
                        Doc.Line,
                        Token.Print(binaryExpressionSyntax.OperatorToken, context),
                        " "
                    );
                }
            }

            var possibleBinary = binaryOnTheRight
                ? binaryExpressionSyntax.Right
                : binaryExpressionSyntax.Left;

            // Put all operators with the same precedence level in the same
            // group. The reason we only need to do this with the `left`
            // expression is because given an expression like `1 + 2 - 3`, it
            // is always parsed like `((1 + 2) - 3)`, meaning the `left` side
            // is where the rest of the expression will exist. Binary
            // expressions on the right side mean they have a difference
            // precedence level and should be treated as a separate group, so
            // print them normally.
            if (
                possibleBinary is BinaryExpressionSyntax childBinary
                && ShouldFlatten(binaryExpressionSyntax.OperatorToken, childBinary.OperatorToken)
            )
            {
                docs.AddRange(PrintBinaryExpression(childBinary, context));
            }
            else
            {
                docs.Add(Node.Print(possibleBinary, context));
            }

            if (binaryOnTheRight)
            {
                return shouldGroup ? [docs[0], Doc.Group(docs.Skip(1).ToList())] : docs;
            }

            var right = Doc.Concat(
                Doc.Line,
                Token.Print(binaryExpressionSyntax.OperatorToken, context),
                " ",
                Node.Print(binaryExpressionSyntax.Right, context)
            );

            docs.Add(shouldGroup ? Doc.Group(right) : right);
            return docs;
        }
        finally
        {
            context.State.PrintingDepth--;
        }
    }

    private static bool ShouldFlatten(SyntaxToken parentToken, SyntaxToken nodeToken)
    {
        return GetPrecedence(parentToken) == GetPrecedence(nodeToken);
    }

    private static int GetPrecedence(SyntaxNode node)
    {
        if (node is BinaryExpressionSyntax binaryExpressionSyntax)
        {
            return GetPrecedence(binaryExpressionSyntax.OperatorToken);
        }

        return -1;
    }

    private static int GetPrecedence(SyntaxToken syntaxToken)
    {
        return syntaxToken.RawSyntaxKind() switch
        {
            SyntaxKind.QuestionQuestionToken => 1,
            SyntaxKind.BarBarToken => 2,
            SyntaxKind.AmpersandAmpersandToken => 3,
            SyntaxKind.BarToken => 4,
            SyntaxKind.CaretToken => 5,
            SyntaxKind.AmpersandToken => 6,
            SyntaxKind.ExclamationEqualsToken => 7,
            SyntaxKind.EqualsEqualsToken => 8,
            SyntaxKind.LessThanToken => 9,
            SyntaxKind.LessThanEqualsToken => 9,
            SyntaxKind.GreaterThanToken => 9,
            SyntaxKind.GreaterThanEqualsToken => 9,
            SyntaxKind.IsKeyword => 9,
            SyntaxKind.AsKeyword => 9,
            SyntaxKind.LessThanLessThanToken => 10,
            SyntaxKind.GreaterThanGreaterThanToken => 10,
            SyntaxKind.GreaterThanGreaterThanGreaterThanToken => 10,
            SyntaxKind.MinusToken => 11,
            SyntaxKind.PlusToken => 11,
            SyntaxKind.AsteriskToken => 12,
            SyntaxKind.SlashToken => 12,
            SyntaxKind.PercentToken => 12,
            _ => throw new Exception($"No precedence defined for {syntaxToken}"),
        };
    }
}
