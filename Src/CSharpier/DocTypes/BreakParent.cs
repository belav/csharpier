namespace CSharpier.DocTypes;

internal sealed class BreakParent : Doc, IBreakParent
{
    public override DocKind Kind => DocKind.BreakParent;
}

internal interface IBreakParent { }
