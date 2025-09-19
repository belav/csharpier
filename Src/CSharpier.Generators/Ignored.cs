using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Generators;

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
        "kindText",
    ];

    public static readonly string[] Types = [nameof(TextSpan), nameof(SyntaxTree)];

    public static readonly Dictionary<string, string[]> PropertiesByType = new()
    {
        { nameof(PropertyDeclarationSyntax), ["semicolon"] },
        { nameof(IndexerDeclarationSyntax), ["semicolon"] },
        { nameof(SyntaxTrivia), ["token"] },
        { nameof(SyntaxToken), ["value", "valueText"] },
        { nameof(ParameterSyntax), ["exclamationExclamationToken"] },
    };

    public static readonly HashSet<string> UnsupportedNodes = [];
}
