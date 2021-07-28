using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Worker
{
    public static class Ignored
    {
        public static string[] Properties =
        {
            "language",
            "parent",
            "parentTrivia",
            "spanStart",
            "rawKind",
            "containsDiagnostics",
            "containsDirectives",
            "isStructuredTrivia",
            "hasStructuredTrivia",
            "containsSkippedText",
            "containsAnnotations"
        };

        public static Type[] Types = { typeof(TextSpan), typeof(SyntaxTree) };

        public static Dictionary<Type, string[]> PropertiesByType =
            new()
            {
                { typeof(PropertyDeclarationSyntax), new[] { "semicolon" } },
                { typeof(IndexerDeclarationSyntax), new[] { "semicolon" } },
                { typeof(SyntaxTrivia), new[] { "token" } },
                { typeof(SyntaxToken), new[] { "value", "valueText" } }
            };
    }
}
