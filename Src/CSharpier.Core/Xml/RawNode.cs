using System.Text.Json.Serialization;
using System.Xml;

namespace CSharpier.Core.Xml;

internal class RawNode
{
    [JsonIgnore]
    public RawNode? Parent { get; set; }

    [JsonIgnore]
    public RawNode? PreviousNode { get; set; }

    [JsonIgnore]
    public RawNode? NextNode { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required XmlNodeType NodeType { get; set; }
    public bool IsEmpty { get; set; }
    public RawAttribute[] Attributes { get; set; } = [];
    public List<RawNode> Nodes { get; set; } = [];
    public string Value { get; set; } = string.Empty;

    public bool IsTextLike()
    {
        return this.NodeType is XmlNodeType.Text or XmlNodeType.Comment or XmlNodeType.CDATA;
    }

    public RawNode GetLastDescendant()
    {
        return this.NodeType is XmlNodeType.Element ? this.Nodes.LastOrDefault() ?? this : this;
    }
}
