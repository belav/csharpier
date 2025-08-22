using System.Xml;

namespace CSharpier.Core.Xml;

internal class RawElementReader(string originalXml, string endOfLine, XmlReader xmlReader)
{
    public static List<RawElement> ReadAllElements(
        string originalXml,
        string endOfLine,
        XmlReader xmlReader
    )
    {
        var elements = new List<RawElement>();
        var elementStack = new Stack<RawElement>();
        var attributeReader = new RawAttributeReader(originalXml, endOfLine, xmlReader);

        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Element)
            {
                var element = new RawElement
                {
                    Name = xmlReader.Name,
                    NodeType = xmlReader.NodeType,
                    IsEmpty = xmlReader.IsEmptyElement,
                    Attributes =
                        xmlReader.AttributeCount > 0 ? attributeReader.GetAttributes() : [],
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

                if (!element.IsEmpty)
                {
                    elementStack.Push(element);
                }
            }
            else if (xmlReader.NodeType == XmlNodeType.EndElement && elementStack.Count > 0)
            {
                elementStack.Pop();
            }
        }

        return elements;
    }
}
