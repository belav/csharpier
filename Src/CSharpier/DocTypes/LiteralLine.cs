using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class LiteralLine : LineDoc, IBreakParent
    {
        public LiteralLine()
        {
            Type = LineType.Hard;
            IsLiteral = true;
        }
    }
}
