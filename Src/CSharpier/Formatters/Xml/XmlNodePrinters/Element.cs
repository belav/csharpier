using System.Xml;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Element
{
    internal static Doc Print(XmlElement element)
    {
        if (element.IsEmpty)
        {
            return Doc.Group("<" + element.Name, PrintAttributes(element), Doc.Line, "/>");
        }

        return Doc.Group(
            Doc.Group("<" + element.Name, PrintAttributes(element), Doc.SoftLine, ">"),
            PrintChildren(element),
            "</" + element.Name + ">"
        );
    }

    private static Doc PrintAttributes(XmlElement element)
    {
        var result = new List<Doc>();
        foreach (XmlAttribute attribute in element.Attributes)
        {
            // TODO XML replace line endings in Value with the proper ones
            result.Add(Doc.Line, attribute.Name, "=\"", attribute.Value, "\"");
        }

        return Doc.Indent(result);
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
