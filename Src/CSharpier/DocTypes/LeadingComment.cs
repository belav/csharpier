namespace CSharpier.DocTypes;

internal sealed class LeadingComment : Doc
{
    public override DocKind Kind => DocKind.LeadingComment;

    public CommentType Type { get; init; }
    public string Comment { get; init; } = string.Empty;
}
