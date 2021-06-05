using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class Concat : Doc
    {
        public IList<Doc> Contents { get; set; }

        public Concat(IList<Doc> contents)
        {
            Contents = contents;
        }
    }
}
