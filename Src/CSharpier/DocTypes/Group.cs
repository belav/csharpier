namespace CSharpier.DocTypes {

public class Group : Doc, IHasContents
{
  public Doc Contents { get; set; } = Doc.Null;
  public bool Break { get; set; }
  public string? GroupId { get; set; }
}

}
