namespace CSharpier.DocTypes;

internal class Region(string text) : Doc
{
    public string Text { get; } = text;
    public bool IsEnd { get; init; }
}
