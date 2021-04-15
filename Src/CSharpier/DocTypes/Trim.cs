namespace CSharpier.DocTypes
{
    public class Trim : Doc
    {
        public static Trim Instance { get; } = new Trim();

        private Trim() { }
    }
}
