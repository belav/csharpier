namespace CSharpier.DocTypes
{
    public class IfBreak : Doc
    {
        public Doc FlatContents { get; set; } = Doc.Null;
        public Doc BreakContents { get; set; } = Doc.Null;
        public string? GroupId { get; set; }
    }
}
