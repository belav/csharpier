using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpier
{
    public class Doc
    {
        public static implicit operator Doc(string value)
        {
            return new StringDoc(value);
        }

        public static NullDoc Null { get; } = NullDoc.Instance;
    }

    public class LiteralLine : Concat
    {
        public LiteralLine()
            : base(
                new List<Doc>
                {
                    new LineDoc
                    {
                        Type = LineDoc.LineType.Hard,
                        IsLiteral = true
                    },
                    new BreakParent()
                }
            ) { }
    }

    public class HardLine : Concat
    {
        public HardLine()
            : base(
                new List<Doc>
                {
                    new LineDoc { Type = LineDoc.LineType.Hard },
                    new BreakParent()
                }
            ) { }
    }

    public class IfBreak : Doc
    {
        public Doc FlatContents { get; set; } = Docs.Null;
        public Doc BreakContents { get; set; } = Docs.Null;
        public string? GroupId { get; set; }
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
        public string? GroupId { get; set; }
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

    // should possibly be used by ternary operator
    public class Align : Doc, IHasContents
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
