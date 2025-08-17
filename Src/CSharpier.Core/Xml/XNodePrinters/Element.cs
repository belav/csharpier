using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Element
{
    internal static Doc Print(XmlReader xmlReader, PrintingContext context)
    {
        var elementName = xmlReader.Name;
        var isEmpty = xmlReader.IsEmptyElement;
        var isStartElement = xmlReader.IsStartElement();
        var shouldHugContent = false;
        var attrGroupId = context.GroupFor("element-attr-group-id");

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

            // if (!node.Attributes().Any() && node.Nodes().ToList() is [XText] and not [XCData])
            // {
            //     return Doc.Null;
            // }

            return Doc.SoftLine;
        }
        ;

        Doc PrintLineAfterChildren()
        {
            if (shouldHugContent)
            {
                return Doc.IfBreak(Doc.SoftLine, "", attrGroupId);
            }

            // if (!node.Attributes().Any() && node.Nodes().ToList() is [XText] and not [XCData])
            // {
            //     return Doc.Null;
            // }
            return Doc.SoftLine;
        }

        Doc PrintElementContent()
        {
            var elementContent = isEmpty
                ? Doc.Null
                : Doc.Concat(
                    ForceBreakContent(xmlReader) ? Doc.BreakParent : "",
                    PrintChildrenDoc(),
                    PrintLineAfterChildren()
                );

            return elementContent;
        }

        return Doc.Group(
            Doc.GroupWithId(attrGroupId, Tag.PrintOpeningTag(xmlReader, context)),
            PrintElementContent(),
            Tag.PrintClosingTag(xmlReader, context)
        );
    }

    private static bool ForceBreakContent(XmlReader node)
    {
        return false;
        // var childNode = node.Nodes().Count() == 1 ? node.Nodes().First() : null;
        //
        // return childNode is not null and (XCData or not XText);
    }
}
