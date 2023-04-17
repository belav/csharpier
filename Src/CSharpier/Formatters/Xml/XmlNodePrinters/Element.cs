namespace CSharpier.Formatters.Xml.XmlNodePrinters;

using System.Xml;

internal static class Element
{
    internal static Doc Print(XmlElement element)
    {
        return Doc.Group(
            "<" + element.Name,
            PrintAttributes(element),
            ">",
            PrintChildren(element),
            "</" + element.Name + ">"
        );
    }

    private static Doc PrintAttributes(XmlElement element)
    {
        var result = new List<Doc>();
        foreach (XmlAttribute attribute in element.Attributes)
        {
            result.Add(" ");
            result.Add(attribute.OuterXml);
        }

        return Doc.Concat(result);
    }

    private static Doc PrintChildren(XmlElement element)
    {
        if (element.ChildNodes.Count == 0)
        {
            return Doc.Null;
        }

        if (element.FirstChild is XmlText)
        {
            return Node.Print(element.FirstChild);
        }

        var result = new List<Doc>();

        foreach (XmlNode xmlNode in element.ChildNodes)
        {
            result.Add(Doc.Line);
            result.Add(Node.Print(xmlNode));
        }

        return Doc.Concat(Doc.BreakParent, Doc.Indent(result), Doc.Line);
    }
}
