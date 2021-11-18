namespace CSharpier.DocTypes;

internal class ForceFlat : Doc, IHasContents
{
    public Doc Contents { get; set; } = Doc.Null;
}
