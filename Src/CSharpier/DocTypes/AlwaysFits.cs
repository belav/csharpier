namespace CSharpier.DocTypes;

internal class AlwaysFits : Doc
{
    public readonly Doc Contents;

    public AlwaysFits(Doc printedTrivia)
    {
        this.Contents = printedTrivia;
    }
}
