using System.Xml.Linq;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class XNodeExtensions
{
    public static XNode GetLastDescendant(this XNode node)
    {
        return node is XElement element ? element.Nodes().LastOrDefault() ?? node : node;
    }

    public static bool IsTextLike(this XNode node)
    {
        return node is XText or XComment;
    }
}
