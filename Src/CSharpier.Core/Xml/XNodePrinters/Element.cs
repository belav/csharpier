using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Element
{
    internal static Doc Print(RawNode rawNode, PrintingContext context)
    {
        var shouldHugContent = false;
        var attrGroupId = context.GroupFor("element-attr-group-id");

        Doc PrintChildrenDoc()
        {
            var childContent = ElementChildren.Print(rawNode, context);

            if (shouldHugContent)
            {
                return Doc.IndentIfBreak(
                    Doc.Concat(PrintLineBeforeChildren(), childContent),
                    attrGroupId
                );
            }

            return Doc.Indent(PrintLineBeforeChildren(), childContent);
        }

        Doc PrintLineBeforeChildren()
        {
            if (shouldHugContent)
            {
                return Doc.IfBreak(Doc.SoftLine, "", attrGroupId);
            }

            if (
                rawNode.Nodes.FirstOrDefault() is
                { NodeType: XmlNodeType.Text, Value: ['\n', ..] or ['\r', ..] }
            )
            {
                return Doc.LiteralLine;
            }

            if (rawNode.Attributes.Length == 0 && rawNode.Nodes is [{ NodeType: XmlNodeType.Text }])
            {
                return Doc.Null;
            }

            if (rawNode.Nodes.Any(o => o.NodeType is XmlNodeType.Text && o.Value.Contains('\n')))
            {
                return Doc.HardLine;
            }

            return Doc.SoftLine;
        }
        ;

        Doc PrintLineAfterChildren()
        {
            if (shouldHugContent)
            {
                return Doc.IfBreak(Doc.SoftLine, "", attrGroupId);
            }

            if (rawNode.Attributes.Length == 0 && rawNode.Nodes is [{ NodeType: XmlNodeType.Text }])
            {
                return Doc.Null;
            }
            return Doc.SoftLine;
        }

        var elementContent =
            rawNode.IsEmpty || rawNode.Nodes.Count == 0
                ? Doc.Null
                : Doc.Concat(
                    ForceBreakContent(rawNode) ? Doc.BreakParent : "",
                    PrintChildrenDoc(),
                    PrintLineAfterChildren()
                );

        return Doc.Group(
            Doc.GroupWithId(attrGroupId, Tag.PrintOpeningTag(rawNode, context)),
            elementContent,
            Tag.PrintClosingTag(rawNode, context)
        );
    }

    private static bool ForceBreakContent(RawNode rawNode)
    {
        var childNode = rawNode.Nodes.Count == 1 ? rawNode.Nodes.First() : null;

        return childNode is not null
            && childNode.NodeType is XmlNodeType.CDATA or not XmlNodeType.Text;
    }
}
