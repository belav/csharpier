using System;
using System.Linq;
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

        // this is inefficient without a stringBuilder, but only used for dev work and changing it to use one isn't straightforward.
        private static string Serialize(Doc document, string indent)
        {
            var newLine = Environment.NewLine;
            var nextIndent = indent + "    ";
            string PrintIndentedDocTree(Doc doc)
            {
                return Serialize(doc, nextIndent);
            }

            string PrintConcat(Concat concatToPrint)
            {
                var result = indent + "Doc.Concat(";
                if (concatToPrint.Contents.Count > 0)
                {
                    result += newLine;
                }

                for (var x = 0; x < concatToPrint.Contents.Count; x++)
                {
                    var printResult = PrintIndentedDocTree(concatToPrint.Contents[x]);
                    result += printResult;
                    if (x < concatToPrint.Contents.Count - 1)
                    {
                        result += "," + newLine;
                    }
                }

                result += newLine + indent + ")";
                return result;
            }

            return document switch
            {
                NullDoc => indent + "Doc.Null",
                StringDoc { IsDirective: true } directive
                  => $"{indent}Doc.Directive({directive.Value.Replace("\"", "\\\"")})",
                StringDoc stringDoc => indent + "\"" + stringDoc.Value.Replace("\"", "\\\"") + "\"",
                HardLine hardLine
                  => indent
                      + "Doc.HardLine"
                      + (hardLine.Squash ? "IfNoPreviousLine" : string.Empty)
                      + (
                          hardLine.SkipBreakIfFirstInGroup
                              ? "SkipBreakIfFirstInGroup"
                              : string.Empty
                      ),
                LiteralLine => indent + "Doc.LiteralLine",
                Concat concat => PrintConcat(concat),
                LineDoc lineDoc
                  => indent
                      + (lineDoc.Type == LineDoc.LineType.Normal ? "Doc.Line" : "Doc.SoftLine"),
                BreakParent => "",
                Align align
                  => $"{indent}Doc.Align({align.Width}, {PrintIndentedDocTree(align.Contents)})",
                Trim => $"{indent}Doc.Trim",
                ForceFlat forceFlat
                  => $"{indent}Doc.ForceFlat({newLine}{PrintIndentedDocTree(forceFlat.Contents)})",
                IndentDoc indentDoc
                  => $"{indent}Doc.Indent({newLine}{PrintIndentedDocTree(indentDoc.Contents)}{newLine}{indent})",
                ConditionalGroup conditionalGroup
                  => $"{indent}Doc.ConditionalGroup({newLine}{PrintIndentedDocTree(conditionalGroup.Contents)}{newLine}{indent})",
                Group group
                  => @$"{indent}Doc.Group{(@group.GroupId != null ? "WithId" : string.Empty)}(
{(@group.GroupId != null ? $"{nextIndent}\"{@group.GroupId}\",{newLine}" : string.Empty)}{PrintIndentedDocTree(@group.Contents)}{newLine}{indent})",
                LeadingComment leadingComment
                  => $"{indent}Doc.LeadingComment(\"{leadingComment.Comment}\", CommentType.{(leadingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})",
                TrailingComment trailingComment
                  => $"{indent}Doc.TrailingComment(\"{trailingComment.Comment}\", CommentType.{(trailingComment.Type == CommentType.SingleLine ? "SingleLine" : "MultiLine")})",
                IndentIfBreak indentIfBreak
                  => @$"{indent}Doc.IndentIfBreak(
{PrintIndentedDocTree(indentIfBreak.FlatContents)},
{nextIndent}""{indentIfBreak.GroupId}"")",
                IfBreak ifBreak
                  => @$"{indent}Doc.IfBreak(
{PrintIndentedDocTree(ifBreak.BreakContents)},
{PrintIndentedDocTree(ifBreak.FlatContents)},
{nextIndent}""{ifBreak.GroupId}"")",
                _ => throw new Exception("Can't handle " + document)
            };
        }
    }
}
