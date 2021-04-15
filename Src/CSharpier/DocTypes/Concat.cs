using System.Collections.Generic;

namespace CSharpier.DocTypes
{
    public class Concat : Doc
    {
        public List<Doc> Contents { get; set; }

        public Concat(List<Doc> contents)
        {
            Contents = contents;
        }
    }
}
