using System;

namespace CSharpier
{
    /// <summary>
    /// Used to create a string representation of a Doc
    /// The string representation can be pasted into DocPrinterTests
    /// </summary>
    public static class DocTreePrinter
    {
        public static string Print(Doc document)
        {
            return PrintDocTree(document, string.Empty);
        }

        private static string PrintDocTree(Doc document, string indent)
        {
            switch (document)
            {
                case NullDoc:
                    return indent + "Doc.Null";
                case StringDoc stringDoc:
                    return indent + "\"" + stringDoc.Value?.Replace(
                        "\"",
                        "\\\""
                    ) + "\"";
                case Concat concat:
                    if (
                        concat.Parts.Count == 2
                        && concat.Parts[0] is LineDoc line
                        && concat.Parts[1] is BreakParent
                    )
                    {
                        return indent + (line.IsLiteral
                            ? "LiteralLine"
                            : "HardLine");
                    }

                    var result = indent + "Concat(";
                    if (concat.Parts.Count > 0)
                    {
                        result += Environment.NewLine;
                    }
                    for (var x = 0; x < concat.Parts.Count; x++)
                    {
                        var printResult = PrintDocTree(
                            concat.Parts[x],
                            indent + "    "
                        );
                        result += printResult;
                        if (x < concat.Parts.Count - 1)
                        {
                            result += "," + Environment.NewLine;
                        }
                    }

                    result += ")";
                    return result;
                case LineDoc lineDoc:
                    return indent + (lineDoc.IsLiteral
                        ? "LiteralLine"
                        : lineDoc.Type == LineDoc.LineType.Normal
                            ? "Line"
                            : lineDoc.Type == LineDoc.LineType.Hard
                                ? "HardLine"
                                : "SoftLine");
                case BreakParent:
                    return "";
                case ForceFlat forceFlat:
                    return indent + "ForceFlat(" + Environment.NewLine + PrintDocTree(
                        forceFlat.Contents,
                        indent + "    "
                    ) + ")";
                case IndentDoc indentDoc:
                    return indent + "Indent(" + Environment.NewLine + PrintDocTree(
                        indentDoc.Contents,
                        indent + "    "
                    ) + ")";
                case Group group:
                    return indent + "Group(" + Environment.NewLine + PrintDocTree(
                        group.Contents,
                        indent + "    "
                    ) + ")";
                case LeadingComment leadingComment:
                    return $"{indent}LeadingComment(\"{leadingComment.Comment}\", CommentType.{(leadingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})";
                case TrailingComment trailingComment:
                    return $"{indent}TrailingComment(\"{trailingComment.Comment}\", CommentType.{(trailingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})";
                case SpaceIfNoPreviousComment:
                    return indent + "SpaceIfNoPreviousComment";
                default:
                    throw new Exception("Can't handle " + document);
            }
        }
    }
}
