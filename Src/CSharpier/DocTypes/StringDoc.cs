namespace CSharpier.DocTypes;

internal class StringDoc(string value, bool isDirective = false) : Doc
{
    public string Value { get; } = value;
    public bool IsDirective { get; } = isDirective;
}
