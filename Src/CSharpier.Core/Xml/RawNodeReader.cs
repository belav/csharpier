using System.Xml;

namespace CSharpier.Core.Xml;

internal static class RawNodeReader
{
    public static List<RawNode> ReadAllNodes(
        string originalXml,
        string endOfLine,
        XmlReader xmlReader
    )
    {
        var elements = new List<RawNode>();
        var elementStack = new Stack<RawNode>();
        var attributeReader = new RawAttributeReader(originalXml, endOfLine, xmlReader);

        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Whitespace)
            {
                continue;
            }
            if (xmlReader.NodeType == XmlNodeType.EndElement && elementStack.Count > 0)
            {
                var theElement = elementStack.Pop();
                for (var x = 0; x < theElement.Nodes.Count; x++)
                {
                    if (x > 0)
                    {
                        theElement.Nodes[x - 1].NextNode = theElement.Nodes[x];
                        theElement.Nodes[x].PreviousNode = theElement.Nodes[x - 1];
                    }

                    if (x == theElement.Nodes.Count - 2)
                    {
                        theElement.Nodes[x].NextNode = theElement.Nodes[x + 1];
                        theElement.Nodes[x + 1].PreviousNode = theElement.Nodes[x];
                    }
                }
            }
            else
            {
                var element = new RawNode
                {
                    Name = xmlReader.Name,
                    NodeType = xmlReader.NodeType,
                    IsEmpty = xmlReader.IsEmptyElement,
                    Attributes =
                        xmlReader.AttributeCount > 0 ? attributeReader.GetAttributes() : [],
                    // TODO 1679 I think line endings need to be accounted for in here
                    Value =
                        xmlReader.NodeType is XmlNodeType.Text ? xmlReader.Value
                        : xmlReader.NodeType is XmlNodeType.Comment
                            ? "<!--" + xmlReader.Value + "-->"
                        : xmlReader.NodeType is XmlNodeType.CDATA
                            ? "<![CDATA[" + xmlReader.Value + "]]>"
                        : xmlReader.NodeType is XmlNodeType.ProcessingInstruction ? xmlReader.Value
                        : null,
                };

                if (elementStack.Count > 0)
                {
                    element.Parent = elementStack.Peek();
                    elementStack.Peek().Nodes.Add(element);
                }
                else
                {
                    elements.Add(element);
                }

                if (xmlReader.NodeType == XmlNodeType.Element && !element.IsEmpty)
                {
                    elementStack.Push(element);
                }
            }
        }

        return elements;
    }
}
