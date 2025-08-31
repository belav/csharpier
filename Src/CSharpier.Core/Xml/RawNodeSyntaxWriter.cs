using System.Text.Json;

namespace CSharpier.Core.Xml;

internal static class RawNodeSyntaxWriter
{
    public static string Write(RawNode rootNode)
    {
        return JsonSerializer.Serialize(rootNode);
    }
}
