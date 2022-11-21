namespace CSharpier.DocTypes;

internal class Concat : Doc
{
    public IList<Doc> Contents { get; set; }

    public Concat(IList<Doc> contents)
    {
        this.Contents = contents;
    }

    public override bool ContainsDirective()
    {
        return this.Contents.Any(o => o.ContainsDirective());
    }
}
