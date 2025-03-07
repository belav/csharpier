namespace CSharpier.DocTypes;

internal sealed class StringDoc(string value, bool isDirective = false) : Doc
{
    public override DocKind Kind => DocKind.String;
    public string Value { get; } = value;
    public bool IsDirective { get; } = isDirective;
}
