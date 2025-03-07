namespace CSharpier.DocTypes;

internal sealed class AlwaysFits(Doc printedTrivia) : Doc
{
    public override DocKind Kind => DocKind.AlwaysFits;
    
    public readonly Doc Contents = printedTrivia;
}
