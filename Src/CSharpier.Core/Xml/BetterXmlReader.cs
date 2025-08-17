using System.Xml;

namespace CSharpier.Core.Xml;

internal class BetterXmlReader(XmlReader xmlReader) : IDisposable
{
    private readonly Stack<(string Name, XmlNodeType NodeType)> elementStack = new();

    public string? ParentElementName =>
        this.elementStack.Count > 0 ? this.elementStack.Peek().Name : null;
    public XmlNodeType? ParentElementNodeType =>
        this.elementStack.Count > 0 ? this.elementStack.Peek().NodeType : null;

    public bool Read()
    {
        var result = xmlReader.Read();
        if (!result)
        {
            return false;
        }

        if (xmlReader.NodeType == XmlNodeType.Element)
        {
            if (!xmlReader.IsEmptyElement)
            {
                this.elementStack.Push((xmlReader.Name, xmlReader.NodeType));
            }
        }
        else if (xmlReader.NodeType == XmlNodeType.EndElement)
        {
            if (this.elementStack.Count > 0)
            {
                this.elementStack.Pop();
            }
        }

        return true;
    }

    public void Dispose()
    {
        xmlReader.Dispose();
    }
}
