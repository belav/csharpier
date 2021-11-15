namespace CSharpier.DocTypes;

internal class IndentDoc : Doc, IHasContents
{
    public Doc Contents { get; set; } = Doc.Null;
}
