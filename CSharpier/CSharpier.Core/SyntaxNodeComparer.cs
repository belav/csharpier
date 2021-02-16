using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Core
{
    public class SyntaxNodeComparer
    {
        private static AreEqualResult Equal = new AreEqualResult { AreEqual = true };

        public AreEqualResult AreEqualIgnoringWhitespace(SyntaxNode left, SyntaxNode right, string path)
        {
            if (left == null && right == null)
            {
                return Equal;
            }

            var type = left?.GetType();
            if (type != right?.GetType())
            {
                return new AreEqualResult
                {
                    AreEqual = false,
                    MismatchedPath = path
                };
            }

            if (left.RawKind != right.RawKind)
            {
                return new AreEqualResult
                {
                    AreEqual = false,
                    MismatchedPath = path + "-RawKind"
                };
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

                var leftValue = propertyInfo.GetValue(left);
                var rightValue = propertyInfo.GetValue(right);

                if (propertyType == typeof(bool))
                {
                    if ((bool) leftValue != (bool) rightValue)
                    {
                        return NotEqual(path, $"{propertyName}({leftValue} != {rightValue})");
                    }
                }
                else if (propertyType == typeof(Int32))
                {
                    if ((int) leftValue != (int) rightValue)
                    {
                        return NotEqual(path, $"{propertyName}({leftValue} != {rightValue})");
                    }
                }
                else if (propertyType == typeof(SyntaxToken))
                {
                    var result = this.AreEqualIgnoringWhitespace((SyntaxToken) leftValue, (SyntaxToken) rightValue, path + "-" + propertyName);
                    if (!result.AreEqual)
                    {
                        return result;
                    }
                }
                else if (propertyType == typeof(SyntaxTrivia))
                {
                    var result = this.AreEqualIgnoringWhitespace((SyntaxTrivia) leftValue, (SyntaxTrivia) rightValue, path + "-" + propertyName);
                    if (!result.AreEqual)
                    {
                        return result;
                    }
                }
                else if (typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType))
                {
                    var result = this.AreEqualIgnoringWhitespace(leftValue as SyntaxNode, rightValue as SyntaxNode, path + "-" + propertyName);
                    if (!result.AreEqual)
                    {
                        return result;
                    }
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>))
                {
                    var leftList = (leftValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var rightList = (rightValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var result = CompareLists(leftList, rightList, path + "-" + propertyName, AreEqualIgnoringWhitespace);
                    if (!result.AreEqual)
                    {
                        return result;
                    }
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>))
                {
                    var leftList = (leftValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var rightList = (rightValue as IEnumerable).Cast<SyntaxNode>().ToList();
                    var result = CompareLists(leftList, rightList, path + "-" + propertyName, AreEqualIgnoringWhitespace);
                    if (!result.AreEqual)
                    {
                        return result;
                    }

                    var getSeparatorsMethod = propertyType.GetMethod("GetSeparators");
                    var leftSeparators = (getSeparatorsMethod.Invoke(leftValue, null) as IEnumerable<SyntaxToken>).ToList();
                    var rightSeparators = (getSeparatorsMethod.Invoke(rightValue, null) as IEnumerable<SyntaxToken>).ToList();
                    
                    var result2 = CompareLists(leftSeparators, rightSeparators, path + "-" + propertyName, AreEqualIgnoringWhitespace);
                    if (!result2.AreEqual)
                    {
                        return result2;
                    }
                }
                else if (propertyType == typeof(SyntaxTokenList))
                {
                    var leftList = (leftValue as IEnumerable).Cast<SyntaxToken>().ToList();
                    var rightList = (rightValue as IEnumerable).Cast<SyntaxToken>().ToList();
                    var result = CompareLists(leftList, rightList, path + "-" + propertyName, AreEqualIgnoringWhitespace);
                    if (!result.AreEqual)
                    {
                        return result;
                    }
                }
                else if (propertyType == typeof(SyntaxTriviaList))
                {
                    var leftList = (leftValue as IEnumerable).Cast<SyntaxTrivia>().ToList();
                    var rightList = (rightValue as IEnumerable).Cast<SyntaxTrivia>().ToList();
                    var result = CompareLists(leftList, rightList, path + "-" + propertyName, AreEqualIgnoringWhitespace);
                    if (!result.AreEqual)
                    {
                        return result;
                    }
                }
                else
                {
                    throw new Exception(propertyType.FullName);
                }
            }

            return Equal;
        }

        private AreEqualResult CompareLists<T>(IList<T> left, IList<T> right, string path, Func<T, T, string, AreEqualResult> comparer)
        {
            if (left.Count != right.Count)
            {
                return NotEqual(path, $"Count({left.Count} != {right.Count})");
            }

            for (var x = 0; x < left.Count; x++)
            {
                var result = comparer(left[x], right[x], path + "[" + x + "]");
                if (!result.AreEqual)
                {
                    return result;
                }
            }

            return Equal;
        }
        
        private AreEqualResult NotEqual(string path, string propertyName)
        {
            return new()
            {
                AreEqual = false,
                MismatchedPath = path + "-" + propertyName
            };
        }

        private AreEqualResult AreEqualIgnoringWhitespace(SyntaxToken left, SyntaxToken right, string path)
        {
            // TODO other stuff in here? properties? or just trivia?
            if (left.Text != right.Text)
            {
                return NotEqual(path, "Text");
            }

            var result = this.AreEqualIgnoringWhitespace(left.LeadingTrivia, right.LeadingTrivia, path + "-LeadingTrivia");
            if (!result.AreEqual)
            {
                return result;
            }
            
            var result2 = this.AreEqualIgnoringWhitespace(left.TrailingTrivia, right.TrailingTrivia, path + "-TrailingTrivia");
            if (!result2.AreEqual)
            {
                return result2;
            }

            return Equal;
        }

        private AreEqualResult AreEqualIgnoringWhitespace(SyntaxTrivia left, SyntaxTrivia right, string path)
        {
            if (left.RawKind != right.RawKind)
            {
                return NotEqual(path, "RawKind");
            }

            if (left.ToString() != right.ToString())
            {
                return NotEqual(path, "ToString");
            }

            return Equal;
        }

        private AreEqualResult AreEqualIgnoringWhitespace(SyntaxTriviaList left, SyntaxTriviaList right, string path)
        {
            var cleanedLeft = left.Where(o => o.Kind() != SyntaxKind.EndOfLineTrivia
                                              && o.Kind() != SyntaxKind.WhitespaceTrivia).ToList();
            var cleanedRight = right.Where(o => o.Kind() != SyntaxKind.EndOfLineTrivia
                                                && o.Kind() != SyntaxKind.WhitespaceTrivia).ToList();
            var result = CompareLists(cleanedLeft, cleanedRight, path, AreEqualIgnoringWhitespace);
            if (!result.AreEqual)
            {
                return result;
            }

            return Equal;
        }
    }

    public struct AreEqualResult
    {
        public bool AreEqual { get; set; }
        public string MismatchedPath { get; set; }
    }
}