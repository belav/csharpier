namespace CSharpier.DocTypes;

internal class IndentDoc : Doc, IHasContents
{
    public Doc Contents { get; set; } = Null;
    public bool Ensure { get; set; }
}
