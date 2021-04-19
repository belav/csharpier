using System;
using CSharpier.DocTypes;

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

        // TODO this should really use a stringbuilder, but right now it is only used for dev work so not super urgent
        private static string PrintDocTree(Doc document, string indent)
        {
            var newLine = Environment.NewLine;
            var nextIndent = indent + "    ";
            string PrintIndentedDocTree(Doc doc)
            {
                return PrintDocTree(doc, nextIndent);
            }
            switch (document)
            {
                case NullDoc:
                    return indent + "Docs.Null";
                case StringDoc stringDoc:
                    return indent
                    + "\""
                    + stringDoc.Value?.Replace("\"", "\\\"")
                    + "\"";
                case HardLineIfNoPreviousLine:
                    return indent + "Docs.HardLineIfNoPreviousLine";
                case HardLine:
                    return indent + "Docs.HardLine";
                case LiteralLine:
                    return indent + "Docs.LiteralLine";
                case Concat concat:
                    var result = indent + "Docs.Concat(";
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
                    return indent
                    + (lineDoc.Type == LineDoc.LineType.Normal
                        ? "Docs.Line"
                        : "Docs.SoftLine");
                case BreakParent:
                    return "";
                case Trim:
                    return "Docs.Trim";
                case ForceFlat forceFlat:
                    return $"{indent}Docs.ForceFlat({newLine}{PrintIndentedDocTree(forceFlat.Contents)})";
                case IndentDoc indentDoc:
                    return $"{indent}Docs.Indent({newLine}{PrintIndentedDocTree(indentDoc.Contents)})";
                case Group group:
                    return @$"{indent}Docs.Group{(group.GroupId != null ? "WithId" : string.Empty)}(
{(group.GroupId != null ? $"{nextIndent}\"{group.GroupId}\",{newLine}" : string.Empty)}{PrintIndentedDocTree(@group.Contents)})";
                case LeadingComment leadingComment:
                    return $"{indent}Docs.LeadingComment(\"{leadingComment.Comment}\", CommentType.{(leadingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})";
                case TrailingComment trailingComment:
                    return $"{indent}Docs.TrailingComment(\"{trailingComment.Comment}\", CommentType.{(trailingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})";
                case IfBreak ifBreak:
                    return @$"{indent}Docs.IfBreak
{PrintIndentedDocTree(ifBreak.BreakContents)},
{PrintIndentedDocTree(ifBreak.FlatContents)},
{nextIndent}""{ifBreak.GroupId}"")";
                default:
                    throw new Exception("Can't handle " + document);
            }
        }
    }
}
