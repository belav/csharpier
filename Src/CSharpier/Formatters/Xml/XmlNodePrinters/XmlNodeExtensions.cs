using System.Xml;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class XmlNodeExtensions
{
    public static XmlNode GetLastDescendant(this XmlNode node)
    {
        return node.LastChild ?? node;
    }

    public static bool IsTextLike(this XmlNode node)
    {
        return node is XmlText or XmlComment;
    }
}
