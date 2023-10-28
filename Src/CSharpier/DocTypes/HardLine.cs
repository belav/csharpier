namespace CSharpier.DocTypes;

internal class HardLine : LineDoc, IBreakParent
{
    public bool SkipBreakIfFirstInGroup { get; }

    public HardLine(bool squash = false, bool skipBreakIfFirstInGroup = false)
    {
        this.Type = LineType.Hard;
        this.Squash = squash;
        this.SkipBreakIfFirstInGroup = skipBreakIfFirstInGroup;
    }
}