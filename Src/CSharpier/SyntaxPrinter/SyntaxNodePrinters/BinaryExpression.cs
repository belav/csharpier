using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BinaryExpression
    {
        // TODO this is a very basic version of the logic found in prettier/src/language-js/print/binaryish.js
        // There are a ton of edge cases that are not yet handled
        public static Doc Print(BinaryExpressionSyntax node)
        {
            return Docs.Group(PrintBinaryExpression(node));
        }

        // TODO 0 kill? runtime repo has files that will fail on deep recursion
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
            if (depth > 200)
            {
                throw new InTooDeepException();
            }

            depth++;
            try
            {
                var docs = new List<Doc>();
                if (node is BinaryExpressionSyntax binaryExpressionSyntax)
                {
                    // Put all operators with the same precedence level in the same
                    // group. The reason we only need to do this with the `left`
                    // expression is because given an expression like `1 + 2 - 3`, it
                    // is always parsed like `((1 + 2) - 3)`, meaning the `left` side
                    // is where the rest of the expression will exist. Binary
                    // expressions on the right side mean they have a difference
                    // precedence level and should be treated as a separate group, so
                    // print them normally.
                    if (
                        binaryExpressionSyntax.Left is BinaryExpressionSyntax left &&
                        ShouldFlatten(
                            binaryExpressionSyntax.OperatorToken,
                            left.OperatorToken
                        )
                    ) {
                        docs.AddRange(PrintBinaryExpression(left));
                    }
                    else
                    {
                        docs.Add(
                            SyntaxNodes.Print(binaryExpressionSyntax.Left)
                        );
                    }

                    docs.Add(
                        " ",
                        SyntaxTokens.Print(
                            binaryExpressionSyntax.OperatorToken
                        ),
                        Docs.Line,
                        SyntaxNodes.Print(binaryExpressionSyntax.Right)
                    );
                }
                else
                {
                    docs.Add(Docs.Group(SyntaxNodes.Print(node)));
                }

                return docs;
            }

            finally
            {
                depth--;
            }
        }

        private static bool ShouldFlatten(
            SyntaxToken parentToken,
            SyntaxToken nodeToken
        ) {
            if (GetPrecedence(parentToken) != GetPrecedence(nodeToken))
            {
                return false;
            }

            return true;
        }

        // this was just thrown together to get the tests we currently have working
        // it is missing a lot of operators
        private static int GetPrecedence(SyntaxToken syntaxToken)
        {
            switch (syntaxToken.Kind())
            {
                case SyntaxKind.BarBarToken:
                    return 3;
                case SyntaxKind.AmpersandAmpersandToken:
                    return 4;
                case SyntaxKind.NotEqualsExpression:
                case SyntaxKind.EqualsEqualsToken:
                    return 7;
                default:
                    return 0;
            }
        }
    }
}
