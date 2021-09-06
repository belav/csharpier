using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Generators
{
    public static class Ignored
    {
        public static readonly string[] Properties =
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

        public static readonly Type[] Types = { typeof(TextSpan), typeof(SyntaxTree) };

        public static readonly Dictionary<Type, string[]> PropertiesByType =
            new()
            {
                { typeof(PropertyDeclarationSyntax), new[] { "semicolon" } },
                { typeof(IndexerDeclarationSyntax), new[] { "semicolon" } },
                { typeof(SyntaxTrivia), new[] { "token" } },
                { typeof(SyntaxToken), new[] { "value", "valueText" } },
                // for some reason the github action uses 3.11.0 of Microsoft.CodeAnalysis, but trying to use that version locally fails
                // these are all the extra properties in 3.11 that don't exist in 3.8
                { typeof(ParenthesizedLambdaExpressionSyntax), new[] { "attributeLists" } },
                { typeof(RecordDeclarationSyntax), new[] { "classOrStructKeyword" } },
                { typeof(SimpleLambdaExpressionSyntax), new[] { "attributeLists" } },
                { typeof(UsingDirectiveSyntax), new[] { "globalKeyword" } }
            };
    }
}
