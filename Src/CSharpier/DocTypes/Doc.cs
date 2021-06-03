using System.Collections.Generic;
using System.Linq;

namespace CSharpier.DocTypes
{
    public class Doc
    {
        public static implicit operator Doc(string value)
        {
            return new StringDoc(value);
        }

        public static NullDoc Null { get; } = NullDoc.Instance;

        public static Doc BreakParent => new BreakParent();

        public static HardLine HardLine => new();

        public static HardLine HardLineSkipBreakIfFirstInGroup => new(false, true);

        public static HardLine HardLineIfNoPreviousLine => new(true);

        public static HardLine HardLineIfNoPreviousLineSkipBreakIfFirstInGroup => new(true, true);

        // TODO all of the Line types can probably turn into proper classes, and be the same instance by type
        public static LiteralLine LiteralLine => new();

        public static LineDoc Line => new() { Type = LineDoc.LineType.Normal };

        public static Trim Trim => Trim.Instance;

        public static LineDoc SoftLine => new() { Type = LineDoc.LineType.Soft };

        public static LeadingComment LeadingComment(string comment, CommentType commentType) =>
            new() { Type = commentType, Comment = comment };

        public static TrailingComment TrailingComment(string comment, CommentType commentType) =>
            new() { Type = commentType, Comment = comment };

        public static Concat Concat(List<Doc> contents) => new(contents);

        public static Concat Concat(IEnumerable<Doc> contents) => new(contents.ToList());

        public static Concat Concat(params Doc[] contents) => new(contents.ToList());

        public static Doc Join(Doc separator, IEnumerable<Doc> array)
        {
            var docs = new List<Doc>();

            var list = array.ToList();

            if (list.Count == 1)
            {
                return list[0];
            }

            for (var x = 0; x < list.Count; x++)
            {
                if (x != 0)
                {
                    docs.Add(separator);
                }

                docs.Add(list[x]);
            }

            return Concat(docs);
        }

        public static ForceFlat ForceFlat(List<Doc> contents) =>
            new() { Contents = contents.Count == 0 ? contents[0] : Concat(contents) };

        public static ForceFlat ForceFlat(params Doc[] contents) =>
            new() { Contents = contents.Length == 0 ? contents[0] : Concat(contents) };

        public static Group Group(List<Doc> contents) =>
            new() { Contents = contents.Count == 1 ? contents[0] : Concat(contents) };

        public static Group GroupWithId(string groupId, params Doc[] contents)
        {
            var group = Group(contents);
            group.GroupId = groupId;
            return group;
        }

        public static Group Group(params Doc[] contents) =>
            new() { Contents = contents.Length == 1 ? contents[0] : Concat(contents) };

        public static IndentDoc Indent(params Doc[] contents) =>
            new() { Contents = contents.Length == 1 ? contents[0] : Concat(contents) };

        public static IndentDoc Indent(List<Doc> contents) => new() { Contents = Concat(contents) };

        public static IfBreak IfBreak(
            Doc breakContents,
            Doc flatContents,
            string? groupId = null
        ) =>
            new()
            {
                FlatContents = flatContents,
                BreakContents = breakContents,
                GroupId = groupId,
            };

        public static IndentIfBreak IndentIfBreak(Doc contents, string groupId) =>
            new(contents, groupId);

        public static Doc Directive(string value) => new StringDoc(value, true);

        public static ConditionalGroup ConditionalGroup(params Doc[] options) => new(options);
    }

    public enum CommentType
    {
        SingleLine,
        MultiLine
    }

    interface IHasContents
    {
        Doc Contents { get; set; }
    }
}
