namespace CSharpier.DocTypes
{
    // should possibly be used by ternary operator
    public class Align : Doc, IHasContents
    {
        public Doc Contents { get; set; } = Doc.Null;
    }
}
