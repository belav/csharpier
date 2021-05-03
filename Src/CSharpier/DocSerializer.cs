using System;
using CSharpier.DocTypes;

namespace CSharpier
{
    /// <summary>
    /// Used to create a string representation of a Doc
    /// The string representation can be pasted into DocPrinterTests
    /// </summary>
    public static class DocSerializer
    {
        public static string Serialize(Doc document)
        {
            return Serialize(document, string.Empty);
        }

        // TODO this should really use a stringBuilder, but right now it is only used for dev work so not super urgent
        private static string Serialize(Doc document, string indent)
        {
            var newLine = Environment.NewLine;
            var nextIndent = indent + "    ";
            string PrintIndentedDocTree(Doc doc)
            {
                return Serialize(doc, nextIndent);
            }
            switch (document)
            {
                case NullDoc:
                    return indent + "Doc.Null";
                case StringDoc stringDoc:
                    return indent +
                    "\"" +
                    stringDoc.Value?.Replace("\"", "\\\"") +
                    "\"";
                case HardLineIfNoPreviousLine:
                    return indent + "Doc.HardLineIfNoPreviousLine";
                case HardLine hardLine:
                    return indent +
                    "Doc.HardLine" +
                    (hardLine.SkipBreakIfFirstInGroup
                        ? "SkipBreakIfFirstInGroup"
                        : string.Empty);
                case LiteralLine:
                    return indent + "Doc.LiteralLine";
                case Concat concat:
                    var result = indent + "Doc.Concat(";
                    if (concat.Contents.Count > 0)
                    {
                        result += newLine;
                    }
                    for (var x = 0; x < concat.Contents.Count; x++)
                    {
                        var printResult = PrintIndentedDocTree(
                            concat.Contents[x]
                        );
                        result += printResult;
                        if (x < concat.Contents.Count - 1)
                        {
                            result += "," + newLine;
                        }
                    }

                    result += ")";
                    return result;
                case LineDoc lineDoc:
                    return indent +
                    (lineDoc.Type == LineDoc.LineType.Normal
                        ? "Doc.Line"
                        : "Doc.SoftLine");
                case BreakParent:
                    return "";
                case Trim:
                    return $"{indent}Doc.Trim";
                case ForceFlat forceFlat:
                    return $"{indent}Doc.ForceFlat({newLine}{PrintIndentedDocTree(forceFlat.Contents)})";
                case IndentDoc indentDoc:
                    return $"{indent}Doc.Indent({newLine}{PrintIndentedDocTree(indentDoc.Contents)})";
                case Group group:
                    return @$"{indent}Doc.Group{(group.GroupId != null ? "WithId" : string.Empty)}(
{(group.GroupId != null ? $"{nextIndent}\"{group.GroupId}\",{newLine}" : string.Empty)}{PrintIndentedDocTree(@group.Contents)})";
                case LeadingComment leadingComment:
                    return $"{indent}Doc.LeadingComment(\"{leadingComment.Comment}\", CommentType.{(leadingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})";
                case TrailingComment trailingComment:
                    return $"{indent}Doc.TrailingComment(\"{trailingComment.Comment}\", CommentType.{(trailingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})";
                case IfBreak ifBreak:
                    return @$"{indent}Doc.IfBreak(
{PrintIndentedDocTree(ifBreak.BreakContents)},
{PrintIndentedDocTree(ifBreak.FlatContents)},
{nextIndent}""{ifBreak.GroupId}"")";
                default:
                    throw new Exception("Can't handle " + document);
            }
        }
    }
}
