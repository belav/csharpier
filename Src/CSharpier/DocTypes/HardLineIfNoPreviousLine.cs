using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class HardLineIfNoPreviousLine : Concat
    {
        public bool SkipBreakIfFirstInGroup { get; init; }

        public HardLineIfNoPreviousLine(bool skipBreakIfFirstInGroup = false)
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
            )
        {
            SkipBreakIfFirstInGroup = skipBreakIfFirstInGroup;
        }
    }
}
