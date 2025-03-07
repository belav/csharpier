namespace CSharpier.DocTypes;

internal sealed class TrailingComment : Doc
{
    public override DocKind Kind => DocKind.TrailingComment;

    public CommentType Type { get; set; }
    public string Comment { get; set; } = string.Empty;
}
