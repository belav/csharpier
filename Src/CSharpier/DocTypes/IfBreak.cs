namespace CSharpier.DocTypes;

internal class IfBreak : Doc
{
    public Doc FlatContents { get; set; } = Null;
    public Doc BreakContents { get; set; } = Null;
    public string? GroupId { get; set; }
}

internal sealed class IndentIfBreak : IfBreak
{
    public IndentIfBreak(Doc contents, string groupId)
    {
        this.BreakContents = Indent(contents);
        this.FlatContents = contents;
        this.GroupId = groupId;
    }
}
