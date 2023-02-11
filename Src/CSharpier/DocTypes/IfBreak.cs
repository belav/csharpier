namespace CSharpier.DocTypes;

internal class IfBreak : Doc
{
    public Doc FlatContents { get; init; } = Null;
    public Doc BreakContents { get; init; } = Null;
    public string? GroupId { get; init; }

    public override bool ContainsDirective()
    {
        return this.FlatContents.ContainsDirective() || this.BreakContents.ContainsDirective();
    }
}

internal class IndentIfBreak : IfBreak
{
    public IndentIfBreak(Doc contents, string groupId)
    {
        this.BreakContents = Indent(contents);
        this.FlatContents = contents;
        this.GroupId = groupId;
    }
}
