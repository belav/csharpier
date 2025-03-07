namespace CSharpier.DocTypes;

internal sealed class Concat(IList<Doc> contents) : Doc
{
    public override DocKind Kind => DocKind.Concat;
    public IList<Doc> Contents { get; set; } = contents;
}
