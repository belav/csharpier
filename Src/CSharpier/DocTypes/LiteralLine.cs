using System.Collections.Generic;

namespace CSharpier.DocTypes {

public class LiteralLine : Concat
{
  public LiteralLine()
    : base(
      new List<Doc>
      {
        new LineDoc { Type = LineDoc.LineType.Hard, IsLiteral = true },
        new BreakParent()
      }
    ) { }
}

}
