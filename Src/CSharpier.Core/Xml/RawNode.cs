using System.Xml;

namespace CSharpier.Core.Xml;

// TODO 1679 call this RawNode instead?
internal class RawNode
{
    public RawNode? Parent { get; set; }
    public RawNode? PreviousNode { get; set; }
    public RawNode? NextNode { get; set; }
    public required string? Name { get; set; }
    public required XmlNodeType NodeType { get; set; }
    public required bool IsEmpty { get; set; }
    public required RawAttribute[] Attributes { get; set; }
    public List<RawNode> Nodes { get; set; } = new();
    public string? Value { get; set; }

    public bool IsTextLike()
    {
        return this.NodeType is XmlNodeType.Text or XmlNodeType.Comment;
    }

    public RawNode GetLastDescendant()
    {
        return this.NodeType is XmlNodeType.Element ? this.Nodes.LastOrDefault() ?? this : this;
    }
}
