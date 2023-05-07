namespace CSharpier.Formatters.Xml.XmlNodePrinters;

using System.Xml;

internal static class Node
{
    internal static Doc Print(XmlNode xmlNode)
    {
        if (xmlNode is XmlDocument xmlDocument)
        {
            var result = new List<Doc>();
            foreach (XmlNode node in xmlDocument.ChildNodes)
            {
                result.Add(Print(node), Doc.HardLine);
            }

            result.Add(Doc.HardLine);

            return Doc.Concat(result);
        }

        if (xmlNode is XmlDeclaration xmlDeclaration)
        {
            return xmlDeclaration.OuterXml;
        }

        if (xmlNode is XmlElement xmlElement)
        {
            return Element.Print(xmlElement);
        }

        if (xmlNode is XmlText)
        {
            return xmlNode.OuterXml;
        }

        if (xmlNode is XmlComment)
        {
            return xmlNode.OuterXml;
        }

        throw new Exception("Need to handle + " + xmlNode);
    }
}
