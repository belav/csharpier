namespace CSharpier.DocTypes
{
    public class NullDoc : Doc
    {
        public static NullDoc Instance { get; } = new();

        private NullDoc() { }
    }
}
