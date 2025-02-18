namespace CSharpier.DocTypes;

internal sealed class TrailingComment : Doc
{
    public CommentType Type { get; set; }
    public string Comment { get; set; } = string.Empty;
}
