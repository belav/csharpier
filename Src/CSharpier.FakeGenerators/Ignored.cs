using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.FakeGenerators;

public static class Ignored
{
    public static readonly string[] Properties =
    [
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
        "containsAnnotations",
    ];

    public static readonly Type[] Types = [typeof(TextSpan), typeof(SyntaxTree)];

    public static readonly Dictionary<Type, string[]> PropertiesByType = new()
    {
        { typeof(PropertyDeclarationSyntax), ["semicolon"] },
        { typeof(IndexerDeclarationSyntax), ["semicolon"] },
        { typeof(SyntaxTrivia), ["token"] },
        { typeof(SyntaxToken), ["value", "valueText"] },
        { typeof(ParameterSyntax), ["exclamationExclamationToken"] },
    };

    public static readonly HashSet<string> UnsupportedNodes = [];
}
