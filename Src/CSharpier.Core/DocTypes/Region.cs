namespace CSharpier.Core.DocTypes;

internal sealed class Region(string text) : Doc
{
    public string Text { get; } = text;
    public bool IsEnd { get; init; }
}
