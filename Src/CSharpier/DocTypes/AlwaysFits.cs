namespace CSharpier.DocTypes;

internal class AlwaysFits(Doc printedTrivia) : Doc
{
    public readonly Doc Contents = printedTrivia;
}
