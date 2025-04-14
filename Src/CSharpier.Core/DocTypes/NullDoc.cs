namespace CSharpier.Core.DocTypes;

internal sealed class NullDoc : Doc
{
    public static NullDoc Instance { get; } = new();

    private NullDoc() { }
}
