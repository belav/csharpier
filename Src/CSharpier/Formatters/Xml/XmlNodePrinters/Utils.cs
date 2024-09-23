using System.Xml;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Utils
{
    public static bool IsLeadingSpaceSensitiveNode(XmlNode node)
    {
        var isLeadingSpaceSensitive = IsLeadingSpaceSensitiveNode2();

        // TODO what is interpolation?
        // if (
        //     isLeadingSpaceSensitive &&
        //     !node.prev &&
        //     node.parent?.tagDefinition?.ignoreFirstLf
        // ) {
        //     return node.type === "interpolation";
        // }

        return isLeadingSpaceSensitive;

        bool IsLeadingSpaceSensitiveNode2()
        {
            // TODO need this?
            // if (isFrontMatter(node) || node.type === "angularControlFlowBlock") {
            //     return false;
            // }

            if (
                (
                    node is XmlText
                // || node.type === "interpolation"

                )
                && (
                    node.PreviousSibling is XmlText
                // || node.prev.type === "interpolation"
                )
            )
            {
                return true;
            }

            if (node.ParentNode is null)
            {
                return false;
            }

            // TODO html
            // if (isPreLikeNode(node.parent)) {
            //     return true;
            // }

            // if (
            //     node.PreviousSibling is null
            //     &&
            //     (node.parent.type == "root" ||
            //                           (isPreLikeNode(node) && node.parent) ||
            //                           isScriptLikeTag(node.parent) ||
            //                           isVueCustomBlock(node.parent, options) ||
            //                           !isFirstChildLeadingSpaceSensitiveCssDisplay(
            //                               node.parent.cssDisplay,
            //                           ))
            // ) {
            //     return false;
            // }

            // if (
            //     node.prev &&
            //     !isNextLeadingSpaceSensitiveCssDisplay(node.prev.cssDisplay)
            // ) {
            //     return false;
            // }

            return true;
        }
    }

    public static IEnumerable<Doc> GetTextValueParts(XmlText xmlText)
    {
        if (xmlText.Value is null)
        {
            yield break;
        }

        yield return xmlText.Value;

        // return node.parent.isWhitespaceSensitive
        //     ? node.parent.isIndentationSensitive
        //         ? replaceEndOfLine(value)
        //         : replaceEndOfLine(
        //             dedentString(htmlTrimPreserveIndentation(value)),
        //             hardline,
        //         )
        //     : join(line, htmlWhitespaceUtils.split(value));
    }
}
