namespace CSharpier.DocTypes;

internal sealed class ForceFlat : Doc, IHasContents
{
    public override DocKind Kind => DocKind.ForceFlat;

    public Doc Contents { get; set; } = Null;
}
