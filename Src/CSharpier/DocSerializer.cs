using System;
using System.Text;
using CSharpier.DocTypes;

namespace CSharpier
{
    /// <summary>
    /// Used to create a string representation of a Doc
    /// The string representation can be pasted into DocPrinterTests
    /// </summary>
    internal static class DocSerializer
    {
        public static string Serialize(Doc doc)
        {
            var result = new StringBuilder();
            Serialize(doc, result, 0);
            return result.ToString();
        }

        public static void Serialize(Doc doc, StringBuilder result, int indent, Doc? parent = null)
        {
            void AppendIndent()
            {
                result.Append(' ', indent * 4);
            }
            void AppendNextIndent()
            {
                result.Append(' ', (indent + 1) * 4);
            }

            if (doc is NullDoc)
            {
                AppendIndent();
                result.Append("Doc.Null");
            }
            else if (doc is Trim)
            {
                AppendIndent();
                result.Append("Doc.Trim");
            }
            else if (doc is StringDoc stringDoc)
            {
                AppendIndent();
                if (stringDoc.IsDirective)
                {
                    result.Append("Doc.Directive(");
                }
                result.Append("\"" + stringDoc.Value.Replace("\"", "\\\"") + "\"");
                if (stringDoc.IsDirective)
                {
                    result.Append(')');
                }
            }
            else if (doc is HardLine hardLine)
            {
                AppendIndent();
                result.Append(
                    "Doc.HardLine"
                        + (hardLine.Squash ? "IfNoPreviousLine" : string.Empty)
                        + (
                            hardLine.SkipBreakIfFirstInGroup
                                ? "SkipBreakIfFirstInGroup"
                                : string.Empty
                        )
                );
            }
            else if (doc is LiteralLine)
            {
                AppendIndent();
                result.Append("Doc.LiteralLine");
            }
            else if (doc is LineDoc lineDoc)
            {
                AppendIndent();
                result.Append(
                    lineDoc.Type == LineDoc.LineType.Normal ? "Doc.Line" : "Doc.SoftLine"
                );
            }
            else if (doc is ConditionalGroup conditionalGroup)
            {
                AppendIndent();
                result.AppendLine("Doc.ConditionalGroup(");

                for (var x = 0; x < conditionalGroup.Options.Length; x++)
                {
                    Serialize(conditionalGroup.Options[x], result, indent + 1);

                    if (x < conditionalGroup.Options.Length - 1)
                    {
                        result.AppendLine(",");
                    }
                }

                result.AppendLine();
                AppendIndent();
                result.Append(')');
            }
            else if (doc is Group group)
            {
                AppendIndent();
                result.AppendLine($"Doc.Group{(group.GroupId != null ? "WithId" : string.Empty)}(");
                if (group.GroupId != null)
                {
                    AppendNextIndent();
                    result.AppendLine($"\"{group.GroupId}\",");
                }
                Serialize(group.Contents, result, indent + 1, group);
                result.AppendLine();
                AppendIndent();
                result.Append(')');
            }
            else if (doc is Concat concat)
            {
                var skipConcat =
                    parent is IHasContents hasContents && hasContents.Contents == concat;

                if (!skipConcat)
                {
                    AppendIndent();
                    result.Append("Doc.Concat(");
                    if (concat.Contents.Count > 0)
                    {
                        result.AppendLine();
                    }
                }

                for (var x = 0; x < concat.Contents.Count; x++)
                {
                    Serialize(concat.Contents[x], result, skipConcat ? indent : indent + 1);

                    if (x < concat.Contents.Count - 1)
                    {
                        result.AppendLine(",");
                    }
                }

                if (!skipConcat)
                {
                    result.AppendLine();
                    AppendIndent();
                    result.Append(')');
                }
            }
            else if (doc is IHasContents hasContents)
            {
                AppendIndent();
                result.AppendLine($"Doc.{(doc is IndentDoc ? "Indent" : doc.GetType().Name)}(");
                if (doc is Align align)
                {
                    AppendNextIndent();
                    result.AppendLine(align.Width + ",");
                }

                Serialize(hasContents.Contents, result, indent + 1, doc);
                result.AppendLine();
                AppendIndent();
                result.Append(')');
            }
            else if (doc is IndentIfBreak indentIfBreak)
            {
                AppendIndent();
                result.AppendLine("Doc.IndentIfBreak(");
                Serialize(indentIfBreak.FlatContents, result, indent + 1, doc);

                result.AppendLine(",");
                AppendNextIndent();
                result.Append($"\"{indentIfBreak.GroupId}\"");

                result.AppendLine();
                AppendIndent();
                result.Append(')');
            }
            else if (doc is IfBreak ifBreak)
            {
                AppendIndent();
                result.AppendLine("Doc.IfBreak(");
                Serialize(ifBreak.BreakContents, result, indent + 1, doc);
                result.AppendLine(",");

                Serialize(ifBreak.FlatContents, result, indent + 1, doc);

                if (ifBreak.GroupId != null)
                {
                    result.AppendLine(",");
                    AppendNextIndent();
                    result.Append($"\"{ifBreak.GroupId}\"");
                }

                result.AppendLine();
                AppendIndent();
                result.Append(')');
            }
            else if (doc is LeadingComment leadingComment)
            {
                AppendIndent();
                result.Append(
                    $"Doc.LeadingComment(\"{leadingComment.Comment}\", CommentType.{leadingComment.Type})"
                );
            }
            else if (doc is TrailingComment trailingComment)
            {
                AppendIndent();
                result.Append(
                    $"Doc.TrailingComment(\"{trailingComment.Comment}\", CommentType.{trailingComment.Type})"
                );
            }
            else
            {
                throw new Exception("Can't handle " + doc);
            }
        }
    }
}
