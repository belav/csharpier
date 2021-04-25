namespace CSharpier.DocTypes {

public class IndentDoc : Doc, IHasContents
{
  public Doc Contents { get; set; } = Doc.Null;
}

}
