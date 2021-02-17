using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Core
{
    public class SyntaxNodeComparer
    {
        protected string OriginalSourceCode { get; }
        protected string NewSourceCode { get; }
        protected SyntaxTree OriginalSyntaxTree { get; }
        protected SyntaxTree NewSyntaxTree { get; }

        private static readonly CompareResult Equal = new CompareResult();
        
        public SyntaxNodeComparer(string originalSourceCode, string newSourceCode)
        {
            this.OriginalSourceCode = originalSourceCode;
            this.NewSourceCode = newSourceCode;
            
            var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp9);
            this.OriginalSyntaxTree = CSharpSyntaxTree.ParseText(this.OriginalSourceCode, cSharpParseOptions);
            this.NewSyntaxTree = CSharpSyntaxTree.ParseText(this.NewSourceCode, cSharpParseOptions);
        }

        public string CompareSource()
        {
            var result = this.AreEqualIgnoringWhitespace(OriginalSyntaxTree.GetRoot(), NewSyntaxTree.GetRoot());
            var message = "";
            if (result.MismatchedResult)
            {
                message += "    Original: " + GetLine(result.OriginalSpan, this.OriginalSyntaxTree, this.OriginalSourceCode);
                
                message += "    New: " + GetLine(result.NewSpan, this.NewSyntaxTree, this.NewSourceCode);
            }

            return message == "" ? null : message;
        }

        public string GetLine(TextSpan? textSpan, SyntaxTree syntaxTree, string source)
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
            var currentLine = stringReader.ReadLine();
            while (x <= endLine + 2 && currentLine != null)
            {
                if (x >= line - 2)
                {
                    result += currentLine + Environment.NewLine;
                }
                
                currentLine = stringReader.ReadLine();
                x++;
            }

            return result;
        }
        
        public CompareResult AreEqualIgnoringWhitespace(SyntaxNode originalNode, SyntaxNode newNode)
        {
            if (originalNode == null && newNode == null)
            {
                return Equal;
            }

            var type = originalNode?.GetType();
            if (type != newNode?.GetType())
            {
                return NotEqual(originalNode, newNode);
            }

            if (originalNode.RawKind != newNode.RawKind)
            {
                return NotEqual(originalNode, newNode);
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyName = propertyInfo.Name;
                if (propertyName == "Language"
                    || propertyName == "Parent"
                    || propertyName == "HasLeadingTrivia" // we modify/remove whitespace and new lines so we can't look at these properties.
                    || propertyName == "HasTrailingTrivia"
                    || propertyName == "ParentTrivia"
                    || propertyName == "Arity"
                    || propertyName == "SpanStart")
                {
                    continue;
                }

                var propertyType = propertyInfo.PropertyType;
                if (propertyType == typeof(TextSpan)
                    || propertyType == typeof(SyntaxTree))
                {
                    continue;
                }

                var originalValue = propertyInfo.GetValue(originalNode);
                var newValue = propertyInfo.GetValue(newNode);

                var result = Equal;
                
                if (propertyType == typeof(bool))
                {
                    if ((bool) originalValue != (bool) newValue)
                    {
                        return NotEqual(originalNode, newNode);
                    }
                }
                else if (propertyType == typeof(Int32))
                {
                    if ((int) originalValue != (int) newValue)
                    {
                        return NotEqual(originalNode, newNode);
                    }
                }
                else if (propertyType == typeof(SyntaxToken))
                {
                    result = this.AreEqualIgnoringWhitespace((SyntaxToken) originalValue, (SyntaxToken) newValue, originalNode, newNode);
                }
                else if (propertyType == typeof(SyntaxTrivia))
                {
                    result = this.AreEqualIgnoringWhitespace((SyntaxTrivia) originalValue, (SyntaxTrivia) newValue);
                }
                else if (typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType))
                {
                    result = this.AreEqualIgnoringWhitespace(originalValue as SyntaxNode, newValue as SyntaxNode);
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>))
                {
                    var leftList = (originalValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var rightList = (newValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    result = CompareLists(leftList, rightList, AreEqualIgnoringWhitespace, o => o.Span);
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>))
                {
                    var leftList = (originalValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var rightList = (newValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    result = CompareLists(leftList, rightList, AreEqualIgnoringWhitespace, o => o.Span);
                    if (result.MismatchedResult)
                    {
                        return result;
                    }
                    
                    var getSeparatorsMethod = propertyType.GetMethod("GetSeparators");
                    var leftSeparators = (getSeparatorsMethod.Invoke(originalValue, null) as IEnumerable<SyntaxToken>).ToList();
                    var rightSeparators = (getSeparatorsMethod.Invoke(newValue, null) as IEnumerable<SyntaxToken>).ToList();
                    
                    result = CompareLists(leftSeparators, rightSeparators, AreEqualIgnoringWhitespace, o => o.Span);
                }
                else if (propertyType == typeof(SyntaxTokenList))
                {
                    var leftList = (originalValue as IEnumerable).Cast<SyntaxToken>().ToList();
                    var rightList = (newValue as IEnumerable).Cast<SyntaxToken>().ToList();
                    result = CompareLists(leftList, rightList, AreEqualIgnoringWhitespace, o => o.Span);
                }
                else if (propertyType == typeof(SyntaxTriviaList))
                {
                    var leftList = (originalValue as IEnumerable).Cast<SyntaxTrivia>().ToList();
                    var rightList = (newValue as IEnumerable).Cast<SyntaxTrivia>().ToList();
                    result = CompareLists(leftList, rightList, AreEqualIgnoringWhitespace, o => o.Span);
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

        private CompareResult CompareLists<T>(IList<T> originalList, IList<T> newList, Func<T, T, CompareResult> comparer, Func<T, TextSpan> getSpan)
        {
            for (var x = 0; x < originalList.Count || x < newList.Count; x++)
            {
                if (x == originalList.Count)
                {
                    return NotEqual(null, getSpan(newList[x]));
                }

                if (x == newList.Count)
                {
                    return NotEqual(getSpan(originalList[x]), null);
                }
                
                var result = comparer(originalList[x], newList[x]);
                if (result.MismatchedResult)
                {
                    return result;
                }
            }

            return Equal;
        }
        
        private CompareResult NotEqual(SyntaxNode originalNode, SyntaxNode newNode)
        {
            return new()
            {
                MismatchedResult = true,
                OriginalSpan = originalNode.Span,
                NewSpan = newNode.Span
            };
        }
        
        private CompareResult NotEqual(TextSpan? originalSpan, TextSpan? newSpan)
        {
            return new()
            {
                MismatchedResult = true,
                OriginalSpan = originalSpan,
                NewSpan = newSpan
            };
        }

        private CompareResult AreEqualIgnoringWhitespace(SyntaxToken originalToken, SyntaxToken newToken)
        {
            return AreEqualIgnoringWhitespace(originalToken, newToken, null, null);
        }
        
        private CompareResult AreEqualIgnoringWhitespace(SyntaxToken originalToken, SyntaxToken newToken, SyntaxNode originalNode, SyntaxNode newNode)
        {
            // TODO other stuff in here? properties? or just trivia?
            if (originalToken.Text != newToken.Text)
            {
                return NotEqual(originalToken.RawKind == 0 ? originalNode.Span : originalToken.Span,
                    newToken.RawKind == 0 ? newNode.Span : newToken.Span
                    );
            }
        
            var result = this.AreEqualIgnoringWhitespace(originalToken.LeadingTrivia, newToken.LeadingTrivia);
            if (result.MismatchedResult)
            {
                return result;
            }
            
            var result2 = this.AreEqualIgnoringWhitespace(originalToken.TrailingTrivia, newToken.TrailingTrivia);
            if (result2.MismatchedResult)
            {
                return result2;
            }
        
            return Equal;
        }

        private CompareResult AreEqualIgnoringWhitespace(SyntaxTrivia originalTrivia, SyntaxTrivia newTrivia)
        {
            if (originalTrivia.ToString().TrimEnd() != newTrivia.ToString().TrimEnd())
            {
                return NotEqual(originalTrivia.Span, newTrivia.Span);
            }

            return Equal;
        }

        private CompareResult AreEqualIgnoringWhitespace(SyntaxTriviaList left, SyntaxTriviaList right)
        {
            var cleanedLeft = left.Where(o => o.Kind() != SyntaxKind.EndOfLineTrivia
                                              && o.Kind() != SyntaxKind.WhitespaceTrivia).ToList();
            var cleanedRight = right.Where(o => o.Kind() != SyntaxKind.EndOfLineTrivia
                                                && o.Kind() != SyntaxKind.WhitespaceTrivia).ToList();
            var result = CompareLists(cleanedLeft, cleanedRight, AreEqualIgnoringWhitespace, o => o.Span);
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