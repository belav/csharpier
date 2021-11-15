namespace CSharpier.DocTypes;

internal class TrailingComment : Doc
{
    public CommentType Type { get; set; }
    public string Comment { get; set; } = string.Empty;
}
