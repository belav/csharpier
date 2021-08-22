using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    // the usage or this is a little wonky right now.
    // the next iteration of this will do the following
    // find all #if and related directives (from file contents)
    // come up with the symbol sets
    // use those in code formatter
    // that will get rid of the ugly threadStatic/reset/stopCollecting stuff
    public class PreprocessorSymbols
    {
        [ThreadStatic]
        protected static List<string[]>? SymbolSets;

        [ThreadStatic]
        private static bool doneCollecting;

        public static void StopCollecting()
        {
            doneCollecting = true;
        }

        public static void Reset()
        {
            SymbolSets?.Clear();
            doneCollecting = false;
        }

        public static void SetSymbolSets(List<string[]> value)
        {
            SymbolSets = value;
        }

        public static void AddSymbolSet(SyntaxTrivia trivia)
        {
            if (doneCollecting)
            {
                return;
            }

            var kind = trivia.Kind();

            if (kind is SyntaxKind.IfDirectiveTrivia or SyntaxKind.ElifDirectiveTrivia)
            {
                var ifDirectiveTriviaSyntax =
                    trivia.GetStructure() as ConditionalDirectiveTriviaSyntax;
                AddSymbolSetForConditional(ifDirectiveTriviaSyntax!);
            }
        }

        protected static void AddSymbolSetForConditional(
            ConditionalDirectiveTriviaSyntax conditionalDirectiveTriviaSyntax
        ) {
            var stack = new Stack<ExpressionSyntax>();
            stack.Push(conditionalDirectiveTriviaSyntax.Condition);

            List<string>? symbolSet = null;

            while (stack.Count > 0)
            {
                var condition = stack.Pop();

                if (condition is ParenthesizedExpressionSyntax parenthesizedExpressionSyntax)
                {
                    stack.Push(parenthesizedExpressionSyntax.Expression);
                }
                else if (condition is PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax)
                {
                    stack.Push(prefixUnaryExpressionSyntax.Operand);
                }
                else if (condition is BinaryExpressionSyntax binaryExpressionSyntax)
                {
                    stack.Push(binaryExpressionSyntax.Left);
                    stack.Push(binaryExpressionSyntax.Right);
                }
                else if (condition is IdentifierNameSyntax identifierNameSyntax)
                {
                    symbolSet ??= new List<string>();

                    if (!symbolSet.Contains(identifierNameSyntax.Identifier.Text))
                    {
                        symbolSet.Add(identifierNameSyntax.Identifier.Text);
                    }
                }
            }

            if (symbolSet != null)
            {
                SymbolSets ??= new();
                var orderedSymbols = symbolSet.OrderBy(o => o).ToArray();
                if (!SymbolSets.Any(o => o.SequenceEqual(orderedSymbols)))
                {
                    SymbolSets.Add(orderedSymbols);
                }
            }
        }

        public static IEnumerable<string[]> GetSymbolSets()
        {
            if (SymbolSets == null)
            {
                yield return Array.Empty<string>();
                yield break;
            }

            foreach (var symbol in SymbolSets)
            {
                yield return symbol;
            }
        }
    }
}
