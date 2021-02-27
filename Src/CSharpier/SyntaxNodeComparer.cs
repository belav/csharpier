using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier
{
    public partial class SyntaxNodeComparer
    {
        protected string OriginalSourceCode { get; }
        protected string NewSourceCode { get; }
        protected SyntaxTree OriginalSyntaxTree { get; }
        protected SyntaxTree NewSyntaxTree { get; }

        private static readonly CompareResult Equal = new();

        public SyntaxNodeComparer(
            string originalSourceCode,
            string newSourceCode)
        {
            this.OriginalSourceCode = originalSourceCode;
            this.NewSourceCode = newSourceCode;

            var cSharpParseOptions = new CSharpParseOptions(
                LanguageVersion.CSharp9);
            this.OriginalSyntaxTree = CSharpSyntaxTree.ParseText(
                this.OriginalSourceCode,
                cSharpParseOptions);
            this.NewSyntaxTree = CSharpSyntaxTree.ParseText(
                this.NewSourceCode,
                cSharpParseOptions);
        }

        public string CompareSource()
        {
            // TODO GH-9 the new generated one fails with stack overflows, look at this for how to potentially avoid recursion
            // can have two stacks, pop the parent off, run compare, the compare method pushes new stuff onto the stack.
            // the stuff on the stack would need an object, and an enum for what type of method to run on the object
            // also maybe some parent data for a couple of the methods
            // http://metacoding.azurewebsites.net/2016/08/16/how-to-avoid-recursion/
            var result = this.AreEqualIgnoringWhitespace(
                OriginalSyntaxTree.GetRoot(),
                NewSyntaxTree.GetRoot());
            var message = "";
            if (result.MismatchedResult)
            {
                message += "    Original: " + GetLine(
                    result.OriginalSpan,
                    this.OriginalSyntaxTree,
                    this.OriginalSourceCode);

                message += "    Formatted: " + GetLine(
                    result.NewSpan,
                    this.NewSyntaxTree,
                    this.NewSourceCode);
            }

            return message == "" ? null : message;
        }

        public string GetLine(
            TextSpan? textSpan,
            SyntaxTree syntaxTree,
            string source)
        {
            if (!textSpan.HasValue)
            {
                return "Missing";
            }

            var line = syntaxTree.GetLineSpan(textSpan.Value).StartLinePosition.Line;
            var endLine = syntaxTree.GetLineSpan(textSpan.Value).EndLinePosition.Line;

            var result = "Around Line " + line + Environment.NewLine;

            var stringReader = new StringReader(source);
            var x = 0;
            var linesWritten = 0;
            var currentLine = stringReader.ReadLine();
            while (x <= endLine + 2 ||
            linesWritten < 8)
            {
                if (x >= line - 2)
                {
                    result += currentLine + Environment.NewLine;
                    linesWritten++;
                }

                currentLine = stringReader.ReadLine();
                if (currentLine == null)
                {
                    break;
                }
                x++;
            }

            return result;
        }

        public CompareResult AreEqualIgnoringWhitespace(
            SyntaxNode originalNode,
            SyntaxNode formattedNode)
        {
            if (originalNode == null && formattedNode == null)
            {
                return Equal;
            }

            var type = originalNode?.GetType();
            if (type != formattedNode?.GetType())
            {
                return NotEqual(originalNode, formattedNode);
            }

            if (originalNode.RawKind != formattedNode.RawKind)
            {
                return NotEqual(originalNode, formattedNode);
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyName = propertyInfo.Name;
                if (
                    propertyName == "Language" ||
                    propertyName == "Parent" ||
                    propertyName == "HasLeadingTrivia" // we modify/remove whitespace and new lines so we can't look at these properties.
                    ||
                    propertyName == "HasTrailingTrivia" ||
                    propertyName == "ParentTrivia" ||
                    propertyName == "Arity" ||
                    propertyName == "SpanStart"
                )
                {
                    continue;
                }

                var propertyType = propertyInfo.PropertyType;
                if (
                    propertyType == typeof(TextSpan) ||
                    propertyType == typeof(SyntaxTree)
                )
                {
                    continue;
                }

                var originalValue = propertyInfo.GetValue(originalNode);
                var formattedValue = propertyInfo.GetValue(formattedNode);

                var result = Equal;

                if (propertyType == typeof(bool))
                {
                    if ((bool)originalValue != (bool)formattedValue)
                    {
                        return NotEqual(originalNode, formattedNode);
                    }
                }
                else if (propertyType == typeof(Int32))
                {
                    if ((int)originalValue != (int)formattedValue)
                    {
                        return NotEqual(originalNode, formattedNode);
                    }
                }
                else if (propertyType == typeof(SyntaxToken))
                {
                    result = this.Compare(
                        (SyntaxToken)originalValue,
                        (SyntaxToken)formattedValue,
                        originalNode,
                        formattedNode);
                }
                else if (propertyType == typeof(SyntaxTrivia))
                {
                    result = this.Compare(
                        (SyntaxTrivia)originalValue,
                        (SyntaxTrivia)formattedValue);
                }
                else if (
                    typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType)
                )
                {
                    var originalValueAsNode = originalValue as SyntaxNode;
                    var formattedValueAsNode = formattedValue as SyntaxNode;
                    if (originalValueAsNode == null && formattedValueAsNode == null)
                    {
                        continue;
                    }

                    if (originalValueAsNode == null || formattedValueAsNode == null)
                    {
                        return NotEqual(originalNode, formattedNode);
                    }
                    result = this.AreEqualIgnoringWhitespace(
                        originalValueAsNode,
                        formattedValueAsNode);
                }
                else if (
                    propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>)
                )
                {
                    var originalList = (originalValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var formattedList = (formattedValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    result = CompareLists(
                        originalList,
                        formattedList,
                        AreEqualIgnoringWhitespace,
                        o => o.Span,
                        originalNode.Span,
                        formattedNode.Span);
                }
                else if (
                    propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>)
                )
                {
                    var originalList = (originalValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var formattedList = (formattedValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    result = CompareLists(
                        originalList,
                        formattedList,
                        AreEqualIgnoringWhitespace,
                        o => o.Span,
                        originalNode.Span,
                        formattedNode.Span);
                    if (result.MismatchedResult)
                    {
                        return result;
                    }

                    var getSeparatorsMethod = propertyType.GetMethod(
                        "GetSeparators");
                    var originalSeparators = (getSeparatorsMethod.Invoke(
                        originalValue,
                        null) as IEnumerable<SyntaxToken>).ToList();
                    var formattedSeparators = (getSeparatorsMethod.Invoke(
                        formattedValue,
                        null) as IEnumerable<SyntaxToken>).ToList();

                    result = CompareLists(
                        originalSeparators,
                        formattedSeparators,
                        Compare,
                        o => o.Span,
                        originalNode.Span,
                        formattedNode.Span);
                }
                else if (propertyType == typeof(SyntaxTokenList))
                {
                    var originalList = (originalValue as IEnumerable).Cast<SyntaxToken>().ToList();
                    var formattedList = (formattedValue as IEnumerable).Cast<SyntaxToken>().ToList();
                    result = CompareLists(
                        originalList,
                        formattedList,
                        Compare,
                        o => o.Span,
                        originalNode.Span,
                        formattedNode.Span);
                }
                else if (propertyType == typeof(SyntaxTriviaList))
                {
                    var originalList = (originalValue as IEnumerable).Cast<SyntaxTrivia>().ToList();
                    var formattedList = (formattedValue as IEnumerable).Cast<SyntaxTrivia>().ToList();
                    result = CompareLists(
                        originalList,
                        formattedList,
                        Compare,
                        o => o.Span,
                        originalNode.Span,
                        formattedNode.Span);
                }
                else
                {
                    throw new Exception(propertyType.FullName);
                }

                if (result.MismatchedResult)
                {
                    return result;
                }
            }

            return Equal;
        }

        private CompareResult CompareLists<T>(
            IReadOnlyList<T> originalList,
            IReadOnlyList<T> formattedList,
            Func<T, T, CompareResult> comparer,
            Func<T, TextSpan> getSpan,
            TextSpan originalParentSpan,
            TextSpan newParentSpan)
        {
            for (var x = 0; x < originalList.Count ||
            x < formattedList.Count; x++)
            {
                if (x == originalList.Count)
                {
                    return NotEqual(
                        originalParentSpan,
                        getSpan(formattedList[x]));
                }

                if (x == formattedList.Count)
                {
                    return NotEqual(getSpan(originalList[x]), newParentSpan);
                }

                var result = comparer(originalList[x], formattedList[x]);
                if (result.MismatchedResult)
                {
                    return result;
                }
            }

            return Equal;
        }

        private CompareResult NotEqual(
            SyntaxNode originalNode,
            SyntaxNode formattedNode)
        {
            return new()
            {
                MismatchedResult = true,
                OriginalSpan = originalNode?.Span,
                NewSpan = formattedNode?.Span
            };
        }

        private CompareResult NotEqual(
            TextSpan? originalSpan,
            TextSpan? formattedSpan)
        {
            return new()
            {
                MismatchedResult = true,
                OriginalSpan = originalSpan,
                NewSpan = formattedSpan
            };
        }

        // TODO 0 this is used by compare lsits, but then we don't have parents if one is missing
        private CompareResult Compare(
            SyntaxToken originalToken,
            SyntaxToken formattedToken)
        {
            return Compare(originalToken, formattedToken, null, null);
        }

        private CompareResult Compare(
            SyntaxToken originalToken,
            SyntaxToken formattedToken,
            SyntaxNode originalNode,
            SyntaxNode formattedNode)
        {
            // TODO other stuff in here? properties? or just trivia?
            if (originalToken.Text != formattedToken.Text)
            {
                return NotEqual(
                    originalToken.RawKind == 0
                        ? originalNode.Span
                        : originalToken.Span,
                    formattedToken.RawKind == 0
                        ? formattedNode.Span
                        : formattedToken.Span);
            }

            var result = this.compare(
                originalToken.LeadingTrivia,
                formattedToken.LeadingTrivia);
            if (result.MismatchedResult)
            {
                return result;
            }

            var result2 = this.compare(
                originalToken.TrailingTrivia,
                formattedToken.TrailingTrivia);
            if (result2.MismatchedResult)
            {
                return result2;
            }

            return Equal;
        }

        private CompareResult Compare(
            SyntaxTrivia originalTrivia,
            SyntaxTrivia formattedTrivia)
        {
            if (
                originalTrivia.ToString().TrimEnd() != formattedTrivia.ToString().TrimEnd()
            )
            {
                return NotEqual(originalTrivia.Span, formattedTrivia.Span);
            }

            return Equal;
        }

        private CompareResult compare(
            SyntaxTriviaList originalList,
            SyntaxTriviaList formattedList)
        {
            var cleanedOriginal = originalList.Where(
                o => o.Kind() != SyntaxKind.EndOfLineTrivia &&
                o.Kind() != SyntaxKind.WhitespaceTrivia).ToList();
            var cleanedFormatted = formattedList.Where(
                o => o.Kind() != SyntaxKind.EndOfLineTrivia &&
                o.Kind() != SyntaxKind.WhitespaceTrivia).ToList();
            var result = CompareLists(
                cleanedOriginal,
                cleanedFormatted,
                Compare,
                o => o.Span,
                originalList.Span,
                formattedList.Span);
            if (result.MismatchedResult)
            {
                return result;
            }

            return Equal;
        }
    }

    public struct CompareResult
    {
        public bool MismatchedResult;
        public TextSpan? OriginalSpan;
        public TextSpan? NewSpan;
    }
}
