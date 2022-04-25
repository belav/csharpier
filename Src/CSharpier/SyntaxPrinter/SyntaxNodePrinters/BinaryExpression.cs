namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

// this is loosely based on prettier/src/language-js/print/binaryish.js
internal static class BinaryExpression
{
    public static Doc Print(BinaryExpressionSyntax node)
    {
        var docs = PrintBinaryExpression(node);

        var shouldNotIndent =
            node.Parent
                // TODO this changes a few things, but fixes one case
                is ArgumentSyntax
                    or ArrowExpressionClauseSyntax
                    or AssignmentExpressionSyntax
                    or CatchFilterClauseSyntax
                    or CheckedExpressionSyntax
                    or DoStatementSyntax
                    or EqualsValueClauseSyntax
                    or IfStatementSyntax
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

    [ThreadStatic]
    private static int depth;

    // The goal of this is to group operators of the same precedence such that they all break or none of them break
    // for example the following should break on the && before it breaks on the !=
    /* (
        one != two
        && three != four
        && five != six
     */
    private static List<Doc> PrintBinaryExpression(SyntaxNode node)
    {
        if (node is not BinaryExpressionSyntax binaryExpressionSyntax)
        {
            return new List<Doc> { Doc.Group(Node.Print(node)) };
        }

        if (depth > 200)
        {
            throw new InTooDeepException();
        }

        depth++;
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
                docs.Add(
                    Node.Print(binaryExpressionSyntax.Left),
                    Doc.Line,
                    Token.Print(binaryExpressionSyntax.OperatorToken),
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
                docs.AddRange(PrintBinaryExpression(childBinary));
            }
            else
            {
                docs.Add(Node.Print(possibleBinary));
            }

            if (binaryOnTheRight)
            {
                return shouldGroup
                  ? new List<Doc> { docs[0], Doc.Group(docs.Skip(1).ToList()) }
                  : docs;
            }

            var right = Doc.Concat(
                Doc.Line,
                Token.Print(binaryExpressionSyntax.OperatorToken),
                " ",
                Node.Print(binaryExpressionSyntax.Right)
            );

            docs.Add(shouldGroup ? Doc.Group(right) : right);
            return docs;
        }
        finally
        {
            depth--;
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
            SyntaxKind.MinusToken => 11,
            SyntaxKind.PlusToken => 11,
            SyntaxKind.AsteriskToken => 12,
            SyntaxKind.SlashToken => 12,
            SyntaxKind.PercentToken => 12,
            _ => throw new Exception($"No precedence defined for {syntaxToken}")
        };
    }
}
