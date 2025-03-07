namespace CSharpier.DocTypes;

internal sealed class NullDoc : Doc
{
    public static NullDoc Instance { get; } = new();

    public override DocKind Kind => DocKind.Null;
}
