namespace CSharpier.DocTypes
{
    public class StringDoc : Doc
    {
        public string Value { get; set; }
        public bool IsDirective { get; set; }

        public StringDoc(string value, bool isDirective = false)
        {
            this.Value = value;
            this.IsDirective = isDirective;
        }
    }
}
