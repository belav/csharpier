using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XNodePrinters;

internal static class Element
{
    internal static Doc Print(XElement node, XmlPrintingContext context)
    {
        var shouldHugContent = false;
        var attrGroupId = context.GroupFor("element-attr-group-id");

        Doc PrintChildrenDoc(params Doc[] childrenDoc)
        {
            if (shouldHugContent)
            {
                return Doc.IndentIfBreak(Doc.Concat(childrenDoc), attrGroupId);
            }

            return Doc.Indent(childrenDoc);
        }

        Doc PrintLineBeforeChildren()
        {
            if (shouldHugContent)
            {
                return Doc.IfBreak(Doc.SoftLine, "", attrGroupId);
            }

            if (!node.Attributes().Any() && node.Nodes().ToList() is [XText])
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

            if (!node.Attributes().Any() && node.Nodes().ToList() is [XText])
            {
                return Doc.Null;
            }
            return Doc.SoftLine;
        }

        var elementContent = !node.Nodes().Any()
            ? Doc.Null
            : Doc.Concat(
                ForceBreakContent(node) ? Doc.BreakParent : "",
                PrintChildrenDoc(PrintLineBeforeChildren(), ElementChildren.Print(node, context)),
                PrintLineAfterChildren()
            );

        return Doc.Group(
            Doc.GroupWithId(attrGroupId, Tag.PrintOpeningTag(node, context)),
            elementContent,
            Tag.PrintClosingTag(node)
        );
    }

    private static bool ForceBreakContent(XElement node)
    {
        var childNode = node.Nodes().Count() == 1 ? node.Nodes().First() : null;

        return childNode is not null && childNode is not XText;
    }
}
