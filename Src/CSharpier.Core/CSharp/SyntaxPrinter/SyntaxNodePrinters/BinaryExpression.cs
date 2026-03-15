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
                (
                    binaryExpressionSyntax.Kind() != SyntaxKind.CoalesceExpression
                    || ShouldGroupNullCoalescingSyntax(binaryExpressionSyntax)
                )
                && binaryExpressionSyntax.Kind() != binaryExpressionSyntax.Parent!.Kind()
                && GetPrecedence(binaryExpressionSyntax)
                    != GetPrecedence(binaryExpressionSyntax.Parent!)
                && binaryExpressionSyntax.Left.GetType() != binaryExpressionSyntax.GetType()
                && binaryExpressionSyntax.Right.GetType() != binaryExpressionSyntax.GetType()
                && binaryExpressionSyntax.Left is not IsPatternExpressionSyntax;

            // nested ?? have the next level on the right side, everything else has it on the left
            var binaryOnTheRight = binaryExpressionSyntax.Kind() == SyntaxKind.CoalesceExpression;
            if (binaryOnTheRight)
            {
                docs.Add(
                    Node.Print(binaryExpressionSyntax.Left, context),
                    Doc.Line,
                    Token.Print(binaryExpressionSyntax.OperatorToken, context),
                    " "
                );
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

    private static bool ShouldGroupNullCoalescingSyntax(
        BinaryExpressionSyntax binaryExpressionSyntax
    )
    {
        if (binaryExpressionSyntax.Kind() != SyntaxKind.CoalesceExpression)
        {
            return false;
        }

        var left = binaryExpressionSyntax.Left;
        return IsTerminalInvocation(left)
            || left is ParenthesizedExpressionSyntax
            || left is CastExpressionSyntax { Expression: ParenthesizedExpressionSyntax }
            || left is SwitchExpressionSyntax;

        static bool IsTerminalInvocation(ExpressionSyntax expression)
        {
            return expression switch
            {
                InvocationExpressionSyntax invocation =>
                    !HasInvocationExcludingLeftmostParenthesized(invocation.Expression),
                ConditionalAccessExpressionSyntax conditionalAccess =>
                    IsTerminalInvocationInWhenNotNull(conditionalAccess.WhenNotNull)
                        && !HasInvocationExcludingLeftmostParenthesized(
                            conditionalAccess.Expression
                        ),
                AwaitExpressionSyntax awaitExpression => IsTerminalInvocation(
                    awaitExpression.Expression
                ),
                _ => false,
            };
        }

        static bool IsTerminalInvocationInWhenNotNull(ExpressionSyntax whenNotNull)
        {
            var rightmost = GetRightmostExpression(whenNotNull);
            if (rightmost is not InvocationExpressionSyntax)
            {
                return false;
            }
            return !whenNotNull
                .DescendantNodesAndSelf()
                .OfType<InvocationExpressionSyntax>()
                .Where(invocation => invocation != rightmost)
                .Any();
        }

        static ExpressionSyntax GetRightmostExpression(ExpressionSyntax expression)
        {
            return expression switch
            {
                InvocationExpressionSyntax invocation => invocation,
                ConditionalAccessExpressionSyntax conditional => GetRightmostExpression(
                    conditional.WhenNotNull
                ),
                MemberAccessExpressionSyntax memberAccess => GetRightmostExpression(
                    memberAccess.Expression
                ),
                _ => expression,
            };
        }

        static bool HasInvocationExcludingLeftmostParenthesized(ExpressionSyntax expression)
        {
            var leftmost = GetLeftmostExpression(expression);
            return expression
                .DescendantNodesAndSelf()
                .Where(node =>
                    leftmost is not ParenthesizedExpressionSyntax excludedNode
                    || !IsDescendantOrSelf(node, excludedNode)
                )
                .Any(node =>
                    node is InvocationExpressionSyntax or ConditionalAccessExpressionSyntax
                );
        }

        static ExpressionSyntax GetLeftmostExpression(ExpressionSyntax expression)
        {
            return expression switch
            {
                MemberAccessExpressionSyntax memberAccess => GetLeftmostExpression(
                    memberAccess.Expression
                ),
                InvocationExpressionSyntax invocation => GetLeftmostExpression(
                    invocation.Expression
                ),
                ConditionalAccessExpressionSyntax conditional => GetLeftmostExpression(
                    conditional.Expression
                ),
                AwaitExpressionSyntax awaitExpr => GetLeftmostExpression(awaitExpr.Expression),
                _ => expression,
            };
        }

        static bool IsDescendantOrSelf(SyntaxNode node, SyntaxNode ancestor)
        {
            return node == ancestor || node.Ancestors().Contains(ancestor);
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
