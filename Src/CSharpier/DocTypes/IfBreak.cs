namespace CSharpier.DocTypes
{
    public class IfBreak : Doc
    {
        public Doc FlatContents { get; set; } = Docs.Null;
        public Doc BreakContents { get; set; } = Docs.Null;
        public string? GroupId { get; set; }
    }
}
