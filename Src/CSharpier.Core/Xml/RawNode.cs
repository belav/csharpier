using System.Xml;

namespace CSharpier.Core.Xml;

// TODO 1679 call this RawNode instead?
internal class RawNode
{
    public RawNode? Parent { get; set; }
    public RawNode? PreviousNode { get; set; }
    public RawNode? NextNode { get; set; }
    public string Name { get; set; } = string.Empty;
    public required XmlNodeType NodeType { get; set; }
    public bool IsEmpty { get; set; }
    public RawAttribute[] Attributes { get; set; } = [];
    public List<RawNode> Nodes { get; set; } = [];
    public string Value { get; set; } = string.Empty;

    public bool IsTextLike()
    {
        return this.NodeType is XmlNodeType.Text or XmlNodeType.Comment;
    }

    public RawNode GetLastDescendant()
    {
        return this.NodeType is XmlNodeType.Element ? this.Nodes.LastOrDefault() ?? this : this;
    }
}
