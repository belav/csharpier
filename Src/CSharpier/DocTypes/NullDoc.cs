namespace CSharpier.DocTypes
{
    public class NullDoc : Doc
    {
        public static NullDoc Instance { get; } = new NullDoc();

        private NullDoc() { }
    }
}
