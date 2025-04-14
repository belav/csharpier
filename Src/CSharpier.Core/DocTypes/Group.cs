namespace CSharpier.Core.DocTypes;

internal class Group : Doc, IHasContents
{
    public Doc Contents { get; set; } = Null;
    public bool Break { get; set; }
    public string? GroupId { get; set; }
}
