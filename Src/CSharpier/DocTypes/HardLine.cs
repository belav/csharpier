using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class HardLine : Concat
    {
        public bool SkipBreakIfFirstInGroup { get; init; }

        public HardLine(
            bool ifNoPreviousLine = false,
            bool skipBreakIfFirstInGroup = false
        )
            : base(
                new List<Doc>
                {
                    new LineDoc
                    {
                        Type = LineDoc.LineType.Hard,
                        Squash = ifNoPreviousLine
                    },
                    new BreakParent(),
                }
            ) {
            SkipBreakIfFirstInGroup = skipBreakIfFirstInGroup;
        }
    }
}
