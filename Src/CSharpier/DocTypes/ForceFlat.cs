namespace CSharpier.DocTypes {

public class ForceFlat : Doc, IHasContents
{
  public Doc Contents { get; set; } = Doc.Null;
}

}
