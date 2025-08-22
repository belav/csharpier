using System.Xml;

namespace CSharpier.Core.Xml;

internal class BetterXmlReader(XmlReader xmlReader) : IDisposable
{
    private readonly Stack<(string Name, XmlNodeType NodeType)> elementStack = new();

    public string? ParentElementName =>
        this.elementStack.Count > 0 ? this.elementStack.Peek().Name : null;
    public XmlNodeType? ParentElementNodeType =>
        this.elementStack.Count > 0 ? this.elementStack.Peek().NodeType : null;

    public XmlNodeType NodeType => xmlReader.NodeType;
    public string Name => xmlReader.Name;
    public bool IsEmptyElement => xmlReader.IsEmptyElement;
    public bool HasValue => xmlReader.HasValue;
    public string Value => xmlReader.Value;
    public int AttributeCount => xmlReader.AttributeCount;

    // TODO 1679 better way to do this?
    public IXmlLineInfo XmlLineInfo => xmlReader as IXmlLineInfo;

    public bool IsStartElement() => xmlReader.IsStartElement();

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

    public BetterXmlReader ReadSubtree()
    {
        return new BetterXmlReader(xmlReader.ReadSubtree());
    }

    public void MoveToFirstAttribute()
    {
        xmlReader.MoveToFirstAttribute();
    }

    public void MoveToNextAttribute()
    {
        xmlReader.MoveToNextAttribute();
    }

    public void MoveToElement()
    {
        xmlReader.MoveToElement();
    }
}
