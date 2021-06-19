using CSharpier.SyntaxPrinter.SyntaxNodePrinters;

namespace CSharpier.DocTypes
{
    public class IfBreak : Doc
    {
        public Doc FlatContents { get; set; } = Doc.Null;
        public Doc BreakContents { get; set; } = Doc.Null;
        public string? GroupId { get; set; }
    }

    public class IfBreakBuilder
    {
        public IfBreak IfBreak { get; } = new();

        public IfBreakBuilder(Doc breakContents, string? groupId)
        {
            IfBreak.BreakContents = breakContents;
            IfBreak.GroupId = groupId;
        }

        public IfBreak Else(Doc flatContents)
        {
            IfBreak.FlatContents = flatContents;
            return IfBreak;
        }
    }

    public class IndentIfBreak : IfBreak
    {
        public IndentIfBreak(Doc contents, string groupId)
        {
            BreakContents = Doc.Indent(contents);
            FlatContents = contents;
            GroupId = groupId;
        }
    }
}
