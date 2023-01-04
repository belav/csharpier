namespace CSharpier;

using System.Text.Json;

internal partial class SyntaxNodeJsonWriter
{
    private static string? WriteBoolean(string name, bool value)
    {
        if (value)
        {
            return $"\"{name}\":true";
        }

        return null;
    }

    private static string? WriteString(string name, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            return $"\"{name}\":\"{JsonEncodedText.Encode(value)}\"";
        }

        return null;
    }

    private static string? WriteInt(string name, int value)
    {
        if (value != 0)
        {
            return $"\"{name}\":{value}";
        }

        return null;
    }

    private static string? GetNodeType(Type type)
    {
        var name = type.Name;
        if (name.EndsWith("Syntax"))
        {
            return name.Substring(0, name.Length - "Syntax".Length);
        }

        return name;
    }
}
