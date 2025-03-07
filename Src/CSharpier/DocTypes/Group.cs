namespace CSharpier.DocTypes;

internal class Group : Doc, IHasContents
{
    public override DocKind Kind => DocKind.Group;
    public Doc Contents { get; set; } = Null;
    public bool Break { get; set; }
    public string? GroupId { get; set; }
}
