namespace CSharpier.Core.DocTypes;

internal class HardLine : LineDoc, IBreakParent
{
    public bool IsForTrivia { get; }

    public HardLine(bool squash = false, bool isForTrivia = false)
    {
        this.Type = LineType.Hard;
        this.Squash = squash;
        this.IsForTrivia = isForTrivia;
    }
}
