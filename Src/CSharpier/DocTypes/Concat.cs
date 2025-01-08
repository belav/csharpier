namespace CSharpier.DocTypes;

internal class Concat(IList<Doc> contents) : Doc
{
    public IList<Doc> Contents { get; set; } = contents;
}
