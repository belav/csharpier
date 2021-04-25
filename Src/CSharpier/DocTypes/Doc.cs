namespace CSharpier.DocTypes {

public class Doc
{
  public static implicit operator Doc(string value)
  {
    return new StringDoc(value);
  }

  public static NullDoc Null { get; } = NullDoc.Instance;
}

public enum CommentType
{
  SingleLine,
  MultiLine
}

interface IHasContents
{
  Doc Contents { get; set; }
}

}
