namespace CSharpier.Core.DocTypes;

internal sealed class AlwaysFits(Doc printedTrivia) : Doc
{
    public readonly Doc Contents = printedTrivia;
}
