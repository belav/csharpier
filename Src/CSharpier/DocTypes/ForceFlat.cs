namespace CSharpier.DocTypes;

internal class ForceFlat : Doc, IHasContents
{
    public Doc Contents { get; init; } = Null;

    public override bool ContainsDirective()
    {
        return this.Contents.ContainsDirective();
    }
}
