using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    // TODO these files fail validation
    // \Newtonsoft.Json\Src\Newtonsoft.Json\Serialization\JsonTypeReflector.cs
    // \aspnetcore\src\Razor\Microsoft.AspNetCore.Razor.Language\src\Legacy\CSharpLanguageCharacteristics.cs
    // \runtime\src\libraries\Common\src\Microsoft\Win32\SafeHandles\SafeX509Handles.Unix.cs
    // \runtime\src\libraries\System.Text.RegularExpressions\src\System\Text\RegularExpressions\RegexBoyerMoore.cs

    // plus some files fail to compile, compare that list

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

            // TODO == false
            // TODO !
            // TODO ||
            // TODO &&
            // TODO ( )
            // TODO complex conditions
            // TODO nested directives
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
