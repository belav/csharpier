namespace CSharpier.DocTypes;

internal class LineDoc : Doc
{
    public override DocKind Kind => DocKind.Line;

    // TODO maybe these should be the DocKinds?
    public enum LineType
    {
        Normal,
        Hard,
        Soft,
    }

    public LineType Type { get; set; }
    public bool IsLiteral { get; set; }
    public bool Squash { get; set; }
}
