namespace CSharpier.DocTypes
{
    public class StringDoc : Doc
    {
        public string Value { get; set; }

        public StringDoc(string value)
        {
            this.Value = value;
        }
    }
}
