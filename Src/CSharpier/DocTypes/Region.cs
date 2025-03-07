namespace CSharpier.DocTypes;

internal sealed class Region(string text) : Doc
{
    public override DocKind Kind => DocKind.Region;
    public string Text { get; } = text;
    public bool IsEnd { get; init; }
}
