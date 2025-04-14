namespace CSharpier.Core.DocTypes;

internal sealed class LiteralLine : LineDoc, IBreakParent
{
    public LiteralLine()
    {
        this.Type = LineType.Hard;
        this.IsLiteral = true;
    }
}
