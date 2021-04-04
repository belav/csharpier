using System.Collections.Generic;
using System.Linq;

namespace CSharpier
{
    public class Doc
    {
        public bool IsHardLine()
        {
            return this is Concat concat
            && concat.Parts.FirstOrDefault() is LineDoc { Type: LineDoc.LineType.Hard } ;
        }

        public static implicit operator Doc(string value)
        {
            return new StringDoc(value);
        }

        public static NullDoc Null { get; } = NullDoc.Instance;
    }

    public class NullDoc : Doc
    {
        public static NullDoc Instance { get; } = new NullDoc();

        private NullDoc() { }
    }

    public class IndentDoc : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
    }

    public class StringDoc : Doc
    {
        public string Value { get; set; }

        public StringDoc(string value)
        {
            this.Value = value;
        }
    }

    public class LineDoc : Doc
    {
        public enum LineType
        {
            Normal,
            Hard,
            Soft
        }

        public LineType Type { get; set; }
        public bool IsLiteral { get; set; }
    }

    public class Group : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
        public bool Break { get; set; }
        public bool ExpandedStates { get; set; }
    }

    public class BreakParent : Doc { }

    public class SpaceIfNoPreviousComment : Doc { }

    public class Concat : Doc
    {
        public List<Doc> Parts { get; set; }

        public Concat(List<Doc> parts)
        {
            Parts = parts;
        }
    }

    public class ForceFlat : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
    }

    public class Align : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
    }

    public class Fill : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
    }

    public class LineSuffix : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
    }

    public class LeadingComment : Doc
    {
        public CommentType Type { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class TrailingComment : Doc
    {
        public CommentType Type { get; set; }
        public string Comment { get; set; } = string.Empty;
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
