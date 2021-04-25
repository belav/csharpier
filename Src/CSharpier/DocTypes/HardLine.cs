using System.Collections.Generic;

namespace CSharpier.DocTypes {

public class HardLine : Concat
{
  public HardLine()
    : base(
      new List<Doc>
      {
        new LineDoc { Type = LineDoc.LineType.Hard },
        new BreakParent()
      }
    ) { }
}

}
