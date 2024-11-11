using System.Xml;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Element
{
    internal static Doc Print(XmlElement node, PrintingContext context)
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

            if (node.Attributes.Count == 0 && node.ChildNodes is [XmlText])
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

            if (node.Attributes.Count == 0 && node.ChildNodes is [XmlText])
            {
                return Doc.Null;
            }
            return Doc.SoftLine;
        }

        var elementContent =
            node.ChildNodes.Count == 0
                ? Doc.Null
                : Doc.Concat(
                    ForceBreakContent(node) ? Doc.BreakParent : "",
                    PrintChildrenDoc(
                        PrintLineBeforeChildren(),
                        ElementChildren.Print(node, context)
                    ),
                    PrintLineAfterChildren()
                );

        return Doc.Group(
            Doc.GroupWithId(attrGroupId, Tag.PrintOpeningTag(node, context)),
            elementContent,
            Tag.PrintClosingTag(node)
        );
    }

    
    
    private static bool ForceBreakContent(XmlElement node)
    {
        var childNode = node.ChildNodes.Count == 1 ? node.ChildNodes[0] : null;

        return childNode is not null && childNode is not XmlText;
    }
}
