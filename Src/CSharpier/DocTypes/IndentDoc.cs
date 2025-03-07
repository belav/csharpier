namespace CSharpier.DocTypes;

internal sealed class IndentDoc : Doc, IHasContents
{
    public override DocKind Kind => DocKind.Indent;

    public Doc Contents { get; set; } = Null;
}
