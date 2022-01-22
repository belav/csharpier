namespace CSharpier.DocTypes;

internal class LiteralLine : LineDoc, IBreakParent
{
    public LiteralLine()
    {
        this.Type = LineType.Hard;
        this.IsLiteral = true;
    }
}
