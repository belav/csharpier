namespace CSharpier.DocTypes;

internal class IndentDoc : Doc, IHasContents
{
    public Doc Contents { get; set; } = Null;

    public override bool ContainsDirective()
    {
        return this.Contents.ContainsDirective();
    }
}
