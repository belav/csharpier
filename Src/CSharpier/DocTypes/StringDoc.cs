namespace CSharpier.DocTypes
{
    public class StringDoc : Doc
    {
        public string Value { get; set; }
        public bool IsTrivia { get; set; }

        public StringDoc(string value, bool isTrivia = false)
        {
            this.Value = value;
            this.IsTrivia = isTrivia;
        }
    }
}
