namespace CSharpier.DocTypes;

internal class IfBreak : Doc
{
    public Doc FlatContents { get; set; } = Doc.Null;
    public Doc BreakContents { get; set; } = Doc.Null;
    public string? GroupId { get; set; }
}

internal class IndentIfBreak : IfBreak
{
    public IndentIfBreak(Doc contents, string groupId)
    {
        BreakContents = Doc.Indent(contents);
        FlatContents = contents;
        GroupId = groupId;
    }
}
