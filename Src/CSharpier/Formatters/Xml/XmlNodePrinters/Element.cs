using System.Xml;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Element
{
    internal static Doc Print(XmlElement node, PrintingContext context)
    {
        var shouldHugContent = false;
        // TODO xml need any of this?
        // node.ChildNodes.Count == 1 &&
        // (node.firstChild.type == "interpolation" ||
        //     node.firstChild.type == "angularIcuExpression") &&
        // node.firstChild.isLeadingSpaceSensitive &&
        // !node.firstChild.hasLeadingSpaces &&
        // node.lastChild.isTrailingSpaceSensitive &&
        // !node.lastChild.hasTrailingSpaces;

        var attrGroupId = context.GroupFor("element-attr-group-id");

        Group PrintTag(Doc doc)
        {
            return Doc.Group(
                Doc.GroupWithId(attrGroupId, Tag.PrintOpeningTag(node, context)),
                doc,
                Tag.PrintClosingTag(node)
            );
        }

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

            // this seems to only apply if whitespace is not strict
            // if (
            //     node.firstChild.hasLeadingSpaces &&
            //     node.firstChild.isLeadingSpaceSensitive
            // ) {
            //     return Doc.Line;
            // }

            if (node.HasChildNodes && node.ChildNodes[0] is XmlText
            // && node.isWhitespaceSensitive
            // && node.isIndentationSensitive
            )
            {
                // TODO xml we don't have dedent?
                // return dedentToRoot(Doc.SoftLine);
            }

            return Doc.SoftLine;
        }
        ;

        Doc PrintLineAfterChildren()
        {
            // var needsToBorrow = node.next
            //     ? needsToBorrowPrevClosingTagEndMarker(node.next)
            //     : needsToBorrowLastChildClosingTagEndMarker(node.parent);
            // if (needsToBorrow) {
            //     if (
            //         node.lastChild.hasTrailingSpaces &&
            //         node.lastChild.isTrailingSpaceSensitive
            //     ) {
            //         return " ";
            //     }
            //     return "";
            // }
            if (shouldHugContent)
            {
                return Doc.IfBreak(Doc.SoftLine, "", attrGroupId);
            }

            // if (
            //     node.lastChild.hasTrailingSpaces &&
            //     node.lastChild.isTrailingSpaceSensitive
            // ) {
            //     return line;
            // }
            // if (
            //     (node.lastChild.type == "comment" ||
            //         (node.lastChild.type == "text" &&
            //             node.isWhitespaceSensitive &&
            //             node.isIndentationSensitive)) &&
            //     new RegExp(
            //         `\\n[\\t ]{${options.tabWidth * (path.ancestors.length - 1)}}$`,
            //     ).test(node.lastChild.value)
            // ) {
            //     return "";
            // }

            return Doc.SoftLine;
        }
        ;

        if (node.ChildNodes.Count == 0)
        {
            return PrintTag(
                ""
            // node.hasDanglingSpaces && node.isDanglingSpaceSensitive ? Doc.Line : ""
            );
        }

        return PrintTag(
            Doc.Concat(
                ForceBreakContent(node) ? Doc.BreakParent : "",
                PrintChildrenDoc(PrintLineBeforeChildren(), ElementChildren.Print(node, context)),
                PrintLineAfterChildren()
            )
        );

        // if (element.IsEmpty)
        // {
        //     return Doc.Doc.Group("<" + element.Name, PrintAttributes(element), Doc.Line, "/>");
        // }
        //
        // return Doc.Doc.Group(
        //     Doc.Doc.Group("<" + element.Name, PrintAttributes(element)),
        //     Doc.Indent(Doc.SoftLine, ">", PrintChildren(element), "</" + element.Name),
        //     Doc.SoftLine,
        //     ">"
        // );

        // return Doc.Doc.Group(
        //     Doc.GroupWithNewId(
        //         out var groupId,
        //         Doc.Indent(
        //             Doc.Doc.Group("<" + element.Name, PrintAttributes(element), Doc.SoftLine, ">"),
        //             PrintChildren(element)
        //         )
        //     ),
        //     "</" + element.Name,
        //     Doc.IfBreak(Doc.SoftLine, Doc.Null, groupId),
        //     ">"
        // );
    }

    private static bool ForceBreakContent(XmlElement node)
    {
        var childNode = node.ChildNodes.Count == 1 ? node.ChildNodes[0] : null;

        return childNode is not null && childNode is not XmlText
        // && HasLeadingLineBreak(childNode)
        // && (!childNode.isTrailingSpaceSensitive || HasTrailingLineBreak(childNode))
        ;
    }

    private static bool HasLeadingLineBreak(XmlNode node)
    {
        return false;
        // return (
        //     node.hasLeadingSpaces &&
        //     (node.prev
        //         ? node.prev.sourceSpan.end.line < node.sourceSpan.start.line
        //         : node.parent.type === "root" ||
        //                                node.parent.startSourceSpan.end.line < node.sourceSpan.start.line)
        // );
    }

    private static bool HasTrailingLineBreak(XmlNode node)
    {
        return false;
        // return (
        //     node.hasTrailingSpaces &&
        //     (node.next
        //         ? node.next.sourceSpan.start.line > node.sourceSpan.end.line
        //         : node.parent.type === "root" ||
        //                                (node.parent.endSourceSpan &&
        //                                 node.parent.endSourceSpan.start.line >
        //                                 node.sourceSpan.end.line))
        // );
    }
}
