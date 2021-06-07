using System;

namespace CSharpier.DocTypes
{
    public class Align : Doc
    {
        public int Alignment { get; }
        public Doc Contents { get; }

        public Align(int alignment, Doc contents)
        {
            if (alignment < 1)
            {
                throw new Exception($"{nameof(alignment)} must be >= 1");
            }

            this.Alignment = alignment;
            this.Contents = contents;
        }
    }
}
