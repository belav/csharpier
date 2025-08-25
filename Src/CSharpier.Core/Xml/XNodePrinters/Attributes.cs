using System.Text;
using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.RawNodePrinters;

internal static class Attributes
{
    public static Doc Print(RawNode rawNode, PrintingContext context)
    {
        if (rawNode.Attributes.Length == 0)
        {
            return rawNode.IsEmpty ? " " : Doc.Null;
        }

        var printedAttributes = new List<Doc>();
        foreach (var attribute in rawNode.Attributes)
        {
            printedAttributes.Add($"{attribute.Name}=\"{attribute.Value}\"");
        }

        var doNotBreakAttributes =
            rawNode.Attributes.Length == 1
            && !rawNode.Attributes[0].Value.Contains('\n')
            && (rawNode.Nodes.Any(o => o.NodeType is XmlNodeType.Element) || rawNode.IsEmpty);
        var attributeLine = Doc.Line;

        var parts = new List<Doc>
        {
            Doc.Indent(
                doNotBreakAttributes ? " " : Doc.Line,
                Doc.Join(attributeLine, printedAttributes)
            ),
        };

        if (
            /*
             *     123<a
             *       attr
             *           ~
             *       >456
             */
            (
                rawNode.Nodes.Count != 0
                && Tag.NeedsToBorrowParentOpeningTagEndMarker(rawNode.Nodes.First())
            ) || doNotBreakAttributes
        )
        {
            parts.Add(rawNode.IsEmpty ? " " : "");
        }
        else
        {
            parts.Add(rawNode.IsEmpty ? Doc.Line : Doc.SoftLine);
        }

        return Doc.Concat(parts);
    }
}
