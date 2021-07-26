using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    // this is loosely based on prettier/src/language-js/print/binaryish.js
    public static class BinaryExpression
    {
        public static Doc Print(BinaryExpressionSyntax node)
        {
            var docs = PrintBinaryExpression(node);

            var shouldNotIndent =
                node.Parent
                    is ReturnStatementSyntax
                        or WhereClauseSyntax
                        or EqualsValueClauseSyntax
                        or ArrowExpressionClauseSyntax
                        or ParenthesizedLambdaExpressionSyntax
                        or AssignmentExpressionSyntax
                        or ConditionalExpressionSyntax
                        or SimpleLambdaExpressionSyntax
                        or IfStatementSyntax
                        or WhileStatementSyntax
                        or SwitchExpressionSyntax
                        or DoStatementSyntax
                        or CheckedExpressionSyntax
                        or CatchFilterClauseSyntax
                        or ParenthesizedExpressionSyntax
                        or SwitchStatementSyntax;

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
                    && binaryExpressionSyntax.Left.GetType() != binaryExpressionSyntax.GetType()
                    && binaryExpressionSyntax.Right.GetType() != binaryExpressionSyntax.GetType();

                // nested ?? have the next level on the right side, everything else has it on the left
                var binaryOnTheRight =
                    binaryExpressionSyntax.Kind() == SyntaxKind.CoalesceExpression;
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
                    && ShouldFlatten(
                        binaryExpressionSyntax.OperatorToken,
                        childBinary.OperatorToken
                    )
                ) {
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

        private static int GetPrecedence(SyntaxToken syntaxToken)
        {
            return syntaxToken.Kind() switch
            {
                SyntaxKind.QuestionQuestionToken => 1,
                SyntaxKind.BarBarToken => 2,
                SyntaxKind.AmpersandAmpersandToken => 3,
                SyntaxKind.BarToken => 4,
                SyntaxKind.CaretToken => 5,
                SyntaxKind.AmpersandToken => 6,
                SyntaxKind.ExclamationEqualsToken => 7,
                SyntaxKind.EqualsEqualsToken => 7,
                SyntaxKind.LessThanToken => 8,
                SyntaxKind.LessThanEqualsToken => 8,
                SyntaxKind.GreaterThanToken => 8,
                SyntaxKind.GreaterThanEqualsToken => 8,
                SyntaxKind.IsKeyword => 8,
                SyntaxKind.AsKeyword => 8,
                SyntaxKind.LessThanLessThanToken => 9,
                SyntaxKind.GreaterThanGreaterThanToken => 9,
                SyntaxKind.MinusToken => 10,
                SyntaxKind.PlusToken => 10,
                SyntaxKind.AsteriskToken => 11,
                SyntaxKind.SlashToken => 11,
                SyntaxKind.PercentToken => 11,
                _ => throw new Exception($"No precedence defined for {syntaxToken}")
            };
        }
    }
}
