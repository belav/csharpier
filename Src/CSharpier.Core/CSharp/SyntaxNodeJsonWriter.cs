using System.Text.Json;

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
}
