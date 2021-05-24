using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class HardLine : LineDoc, IBreakParent
    {
        public bool SkipBreakIfFirstInGroup { get; }

        public HardLine(bool squash = false, bool skipBreakIfFirstInGroup = false)
        {
            Type = LineType.Hard;
            Squash = squash;
            SkipBreakIfFirstInGroup = skipBreakIfFirstInGroup;
        }
    }
}
