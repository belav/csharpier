using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class HardLine : Concat
    {
        public bool SkipBreakIfFirstInGroup { get; init; }

        public HardLine(
            bool squash = false,
            bool skipBreakIfFirstInGroup = false
        )
            : base(
                new List<Doc>
                {
                    new LineDoc
                    {
                        Type = LineDoc.LineType.Hard,
                        Squash = squash
                    },
                    new BreakParent(),
                }
            ) {
            SkipBreakIfFirstInGroup = skipBreakIfFirstInGroup;
        }
    }
}
