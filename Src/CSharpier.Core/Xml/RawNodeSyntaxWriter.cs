using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpier.Core.Xml;

internal static class RawNodeSyntaxWriter
{
    public static string Write(RawNode rootNode)
    {
        return JsonSerializer.Serialize(rootNode, RawNodeJsonContext.Default.RawNode);
    }
}

[JsonSerializable(typeof(RawNode))]
internal sealed partial class RawNodeJsonContext : JsonSerializerContext;
