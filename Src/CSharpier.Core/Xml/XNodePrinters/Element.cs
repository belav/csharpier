using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Element
{
    internal static Doc Print(BetterXmlReader xmlReader, XmlPrintingContext context)
    {
        var rawElement = new RawElement
        {
            Name = xmlReader.Name,
            NodeType = xmlReader.NodeType,
            IsEmpty = xmlReader.IsEmptyElement,
            Attributes = context.RawAttributeReader.GetAttributes(xmlReader),
        };

        var shouldHugContent = false;
        var attrGroupId = context.GroupFor("element-attr-group-id");

        // TODO 1679 somewhere in here we should return the list of RawElements
        // TODO 1679 or we should just convert the reader into RawElements + attributes + etc

        Doc PrintChildrenDoc()
        {
            using var childrenReader = xmlReader.ReadSubtree();
            childrenReader.Read(); // starts at none so read that
            childrenReader.Read(); // then read past the current element to get to the children

            var childContent = ElementChildren.Print(childrenReader, context);

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

            // if (
            //     node.Nodes().FirstOrDefault()
            //     is not XCData
            //         and XText { Value: ['\n', ..] or ['\r', ..] }
            // )
            // {
            //     return Doc.LiteralLine;
            // }

            if (rawElement.Attributes.Length == 0
            // TODO 1679 && node.Nodes().ToList() is [XText] and not [XCData]
            )
            {
                return Doc.Null;
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

            if (rawElement.Attributes.Length == 0
            // TODO 1679 && node.Nodes().ToList() is [XText] and not [XCData]
            )
            {
                return Doc.Null;
            }
            return Doc.SoftLine;
        }

        Doc PrintElementContent()
        {
            var elementContent = rawElement.IsEmpty
                ? Doc.Null
                : Doc.Concat(
                    ForceBreakContent(xmlReader) ? Doc.BreakParent : "",
                    PrintChildrenDoc(),
                    PrintLineAfterChildren()
                );

            return elementContent;
        }

        return Doc.Group(
            Doc.GroupWithId(
                attrGroupId,
                Tag.PrintOpeningTag(xmlReader, rawElement.Attributes, context)
            ),
            PrintElementContent(),
            Tag.PrintClosingTag(xmlReader, context)
        );
    }

    private static bool ForceBreakContent(BetterXmlReader node)
    {
        return false;
        // var childNode = node.Nodes().Count() == 1 ? node.Nodes().First() : null;
        //
        // return childNode is not null and (XCData or not XText);
    }
}
