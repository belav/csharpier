namespace CSharpier.DocTypes;

internal class StringDoc : Doc
{
    public string Value { get; }
    public bool IsDirective { get; }

    public StringDoc(string value, bool isDirective = false)
    {
        this.Value = value;
        this.IsDirective = isDirective;
    }
}
