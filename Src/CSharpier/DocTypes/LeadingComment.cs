namespace CSharpier.DocTypes;

internal class LeadingComment : Doc
{
    public CommentType Type { get; init; }
    public string Comment { get; init; } = string.Empty;

    public override bool ContainsDirective()
    {
        return false;
    }
}
