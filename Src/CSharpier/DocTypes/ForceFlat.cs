namespace CSharpier.DocTypes;

internal sealed class ForceFlat : Doc, IHasContents
{
    public Doc Contents { get; set; } = Null;
}
