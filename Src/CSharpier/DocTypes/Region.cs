namespace CSharpier.DocTypes;

internal class Region : Doc
{
    public Region(string text)
    {
        this.Text = text;
    }

    public string Text { get; }
    public bool IsEnd { get; init; }
}
