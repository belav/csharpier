namespace CSharpier.DocTypes
{
    internal class LeadingComment : Doc
    {
        public CommentType Type { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
