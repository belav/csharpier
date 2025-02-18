namespace CSharpier.DocTypes;

internal sealed class LeadingComment : Doc
{
    public CommentType Type { get; init; }
    public string Comment { get; init; } = string.Empty;
}
