using System.Xml;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

// TODO it may be worth looking
// at how this works instead of the html one
// https://github.com/prettier/plugin-xml/blob/main/src/printer.js
internal static class Element
{
    internal static Doc Print(XmlElement node)
    {
        var shouldHugContent = false;
        // TODO need any of this?
        // node.ChildNodes.Count == 1 &&
        // (node.firstChild.type == "interpolation" ||
        //     node.firstChild.type == "angularIcuExpression") &&
        // node.firstChild.isLeadingSpaceSensitive &&
        // !node.firstChild.hasLeadingSpaces &&
        // node.lastChild.isTrailingSpaceSensitive &&
        // !node.lastChild.hasTrailingSpaces;

        // TODO good idea for group names
        var attrGroupId = Guid.NewGuid().ToString(); // Symbol("element-attr-group-id");

        Group PrintTag(Doc doc) =>
            Doc.Group(
                Doc.GroupWithId(attrGroupId, PrintOpeningTag(node)),
                doc,
                PrintClosingTag(node)
            );

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

            // if (
            //     node.firstChild.hasLeadingSpaces &&
            //     node.firstChild.isLeadingSpaceSensitive
            // ) {
            //     return Doc.Line;
            // }

            // if (
            //     node.firstChild.type == "text" &&
            //     node.isWhitespaceSensitive &&
            //     node.isIndentationSensitive
            // ) {
            //     return dedentToRoot(Doc.SoftLine);
            // }

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
                PrintChildrenDoc(PrintLineBeforeChildren(), ElementChildren.Print(node)),
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

    private static Doc PrintOpeningTag(XmlElement node)
    {
        return Doc.Concat("<" + node.Name, PrintAttributes(node), node.IsEmpty ? Doc.Null : ">");

        // return [
        //     printOpeningTagStart(node, options),
        //     printAttributes(path, options, print),
        //     node.isSelfClosing ? "" : printOpeningTagEnd(node),
        // ];
    }

    private static Doc PrintClosingTag(XmlElement node)
    {
        return Doc.Concat(
            node.IsEmpty ? Doc.Null : PrintClosingTagStart(node),
            PrintClosingTagEnd(node)
        );
        // return [
        //     node.isSelfClosing ? "" : printClosingTagStart(node, options),
        //     printClosingTagEnd(node, options),
        // ];
    }

    private static Doc PrintClosingTagStart(XmlElement node)
    {
        // return node.lastChild is not null &&
        //        needsToBorrowParentClosingTagStartMarker(node.lastChild)
        //     ? Doc.Null
        //     :

        return Doc.Concat(PrintClosingTagPrefix(node), PrintClosingTagStartMarker(node));
    }

    private static Doc PrintClosingTagStartMarker(XmlElement node)
    {
        // if (shouldNotPrintClosingTag(node)) {
        //     return "";
        // }
        // switch (node.type) {
        //     case "ieConditionalComment":
        //         return "<!";
        //     case "element":
        //         if (node.hasHtmComponentClosingTag) {
        //             return "<//";
        //         }
        //     // fall through
        //     default:
        return "</" + node.Name;
    }

    private static Doc PrintClosingTagEnd(XmlElement node)
    {
        // return (
        //     node.next
        //         ? needsToBorrowPrevClosingTagEndMarker(node.next)
        //         : needsToBorrowLastChildClosingTagEndMarker(node.parent)
        // )
        //     ? ""
        //     : [
        //         printClosingTagEndMarker(node, options),
        //         printClosingTagSuffix(node, options),
        //     ];

        return Doc.Concat(PrintClosingTagEndMarker(node), PrintClosingTagSuffix(node));
    }

    private static Doc PrintClosingTagEndMarker(XmlElement node)
    {
        // if (shouldNotPrintClosingTag(node, options)) {
        //     return "";
        // }
        // switch (node.type) {
        //     case "ieConditionalComment":
        //     case "ieConditionalEndComment":
        //         return "[endif]-->";
        //     case "ieConditionalStartComment":
        //         return "]><!-->";
        //     case "interpolation":
        //         return "}}";
        //     case "angularIcuExpression":
        //         return "}";
        //     case "element":
        //         if (node.isSelfClosing) {
        //             return "/>";
        //         }
        //     // fall through
        //     default:

        return node.IsEmpty ? "/>" : ">";
    }

    private static Doc PrintClosingTagSuffix(XmlElement node)
    {
        return Doc.Null;
        // return needsToBorrowParentClosingTagStartMarker(node)
        //     ? printClosingTagStartMarker(node.parent, options)
        //     : needsToBorrowNextOpeningTagStartMarker(node)
        //         ? printOpeningTagStartMarker(node.next)
        //         : "";
    }

    private static Doc PrintClosingTagPrefix(XmlElement node)
    {
        return Doc.Null;
        // return needsToBorrowLastChildClosingTagEndMarker(node)
        //     ? printClosingTagEndMarker(node.lastChild, options)
        //     : "";
    }

    private static bool ForceBreakContent(XmlElement node)
    {
        return false;
        // return (
        //     forceBreakChildren(node) ||
        //     (node.type === "element" &&
        //                    node.children.length > 0 &&
        //                    (["body", "script", "style"].includes(node.name) ||
        //                     node.children.some((child) => hasNonTextChild(child)))) ||
        //     (node.firstChild &&
        //         node.firstChild === node.lastChild &&
        //         node.firstChild.type !== "text" &&
        //                                  hasLeadingLineBreak(node.firstChild) &&
        //                                  (!node.lastChild.isTrailingSpaceSensitive ||
        //                                   hasTrailingLineBreak(node.lastChild)))
        // );
    }

    private static Doc PrintAttributes(XmlElement node)
    {
        if (node.Attributes.Count == 0)
        {
            return node.IsEmpty ? " " : Doc.Null;
        }

        // this is just shoved in here for now
        var result = new List<Doc>();
        foreach (XmlAttribute attribute in node.Attributes)
        {
            result.Add(Doc.Line, attribute.Name, "=\"", attribute.Value, "\"");
        }

        return Doc.Indent(result);
        // const ignoreAttributeData =
        //     node.prev?.type === "comment" &&
        //     getPrettierIgnoreAttributeCommentData(node.prev.value);
        //
        // const hasPrettierIgnoreAttribute =
        //     typeof ignoreAttributeData === "boolean"
        //         ? () => ignoreAttributeData
        //         : Array.isArray(ignoreAttributeData)
        //           ? (attribute) => ignoreAttributeData.includes(attribute.rawName)
        //           : () => false;
        //
        // const printedAttributes = path.map(
        //     ({ node: attribute }) =>
        //         hasPrettierIgnoreAttribute(attribute)
        //             ? replaceEndOfLine(
        //                   options.originalText.slice(
        //                       locStart(attribute),
        //                       locEnd(attribute),
        //                   ),
        //               )
        //             : print(),
        //     "attrs",
        // );
        //
        // const forceNotToBreakAttrContent =
        //     node.type === "element" &&
        //     node.fullName === "script" &&
        //     node.attrs.length === 1 &&
        //     node.attrs[0].fullName === "src" &&
        //     node.children.length === 0;
        //
        // const shouldPrintAttributePerLine =
        //     options.singleAttributePerLine &&
        //     node.attrs.length > 1 &&
        //     !isVueSfcBlock(node, options);
        // const attributeLine = shouldPrintAttributePerLine ? hardline : line;
        //
        // /** @type {Doc[]} */
        // const parts = [
        //     indent([
        //         forceNotToBreakAttrContent ? " " : line,
        //         join(attributeLine, printedAttributes),
        //     ]),
        // ];
        //
        // if (
        //     /**
        //      *     123<a
        //      *       attr
        //      *           ~
        //      *       >456
        //      */
        //     (node.firstChild &&
        //         needsToBorrowParentOpeningTagEndMarker(node.firstChild)) ||
        //     /**
        //      *     <span
        //      *       >123<meta
        //      *                ~
        //      *     /></span>
        //      */
        //     (node.isSelfClosing &&
        //         needsToBorrowLastChildClosingTagEndMarker(node.parent)) ||
        //     forceNotToBreakAttrContent
        // ) {
        //     parts.push(node.isSelfClosing ? " " : "");
        // } else {
        //     parts.push(
        //         options.bracketSameLine
        //             ? node.isSelfClosing
        //                 ? " "
        //                 : ""
        //             : node.isSelfClosing
        //               ? line
        //               : softline,
        //     );
        // }
        //
        // return parts;
    }
}
