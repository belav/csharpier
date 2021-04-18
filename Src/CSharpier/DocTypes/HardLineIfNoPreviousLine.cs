using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class HardLineIfNoPreviousLine : Concat
    {
        public HardLineIfNoPreviousLine()
            : base(
                new List<Doc>
                {
                    new LineDoc
                    {
                        Type = LineDoc.LineType.Hard,
                        Squash = true,
                    },
                    new BreakParent()
                }
            ) { }
    }
}
