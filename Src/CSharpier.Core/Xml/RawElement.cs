using System.Xml;

namespace CSharpier.Core.Xml;

// TODO 1679 call this RawNode instead?
internal class RawElement
{
    public RawElement? Parent { get; set; }

    // TODO 1679 populate these two
    public RawElement? PreviousNode { get; set; }
    public RawElement? NextNode { get; set; }
    public required string? Name { get; set; }
    public required XmlNodeType NodeType { get; set; }
    public required bool IsEmpty { get; set; }
    public required RawAttribute[] Attributes { get; set; }
    public List<RawElement> Nodes { get; set; } = new();

    // TODO 1679 need to populate this for some elements (really nodes)
    public string Value { get; set; } = string.Empty;

    public bool IsTextLike()
    {
        return this.NodeType is XmlNodeType.Text or XmlNodeType.Comment;
    }

    // TODO 1679 do this
    public static RawElement GetLastDescendant(RawElement node)
    {
        return node;
        // return node is XElement element ? element.Nodes().LastOrDefault() ?? node : node;
    }
}
