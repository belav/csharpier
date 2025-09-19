using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Core.CSharp;

internal static partial class SyntaxNodeJsonWriter
{
    private static string? WriteBoolean(string name, bool value)
    {
        return value ? $"\"{name}\":true" : null;
    }

    private static string? WriteString(string name, string value)
    {
        return !string.IsNullOrEmpty(value)
            ? $"\"{name}\":\"{JsonEncodedText.Encode(value)}\""
            : null;
    }

    private static string? WriteInt(string name, int value)
    {
        return value != 0 ? $"\"{name}\":{value}" : null;
    }

    private static string GetNodeType(Type type)
    {
        var name = type.Name;
        return name.EndsWith("Syntax", StringComparison.Ordinal) ? name[..^"Syntax".Length] : name;
    }

    public static void WriteSyntaxToken(StringBuilder builder, SyntaxToken syntaxNode)
    {
        builder.Append('{');
        var properties = new List<string?>
        {
            $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\"",
            $"\"kind\":\"{syntaxNode.Kind()}\"",
            WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia),
            WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia),
            WriteBoolean("isMissing", syntaxNode.IsMissing),
        };
        var leadingTrivia = new List<string>();
        foreach (var node in syntaxNode.LeadingTrivia)
        {
            var innerBuilder = new StringBuilder();
            WriteSyntaxTrivia(innerBuilder, node);
            leadingTrivia.Add(innerBuilder.ToString());
        }
        properties.Add($"\"leadingTrivia\":[{string.Join(",", leadingTrivia)}]");
        properties.Add(WriteString("text", syntaxNode.Text)!);
        var trailingTrivia = new List<string>();
        foreach (var node in syntaxNode.TrailingTrivia)
        {
            var innerBuilder = new StringBuilder();
            WriteSyntaxTrivia(innerBuilder, node);
            trailingTrivia.Add(innerBuilder.ToString());
        }
        properties.Add($"\"trailingTrivia\":[{string.Join(",", trailingTrivia)}]");
        builder.Append(string.Join(",", properties.Where(o => o != null)));
        builder.Append('}');
    }

    public static void WriteSyntaxTrivia(StringBuilder builder, SyntaxTrivia syntaxNode)
    {
        builder.Append('{');
        var properties = new List<string?>
        {
            $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\"",
            $"\"kind\":\"{syntaxNode.Kind()}\"",
            WriteString("text", syntaxNode.ToString()),
            WriteBoolean("hasStructure", syntaxNode.HasStructure),
            WriteBoolean("isDirective", syntaxNode.IsDirective),
        };
        builder.Append(string.Join(",", properties.Where(o => o != null)));
        builder.Append('}');
    }
}
