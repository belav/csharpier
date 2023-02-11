namespace CSharpier.DocTypes;

internal class BreakParent : Doc, IBreakParent
{
    public override bool ContainsDirective()
    {
        return false;
    }
}

internal interface IBreakParent { }
