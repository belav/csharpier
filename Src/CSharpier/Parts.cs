using System.Collections.Generic;

namespace CSharpier
{
    public class Parts : List<Doc>
    {
        public Parts() { }

        public Parts(params Doc[] parts)
        {
            this.AddRange(parts);
        }

        public void Push(params Doc[] value)
        {
            if (value.Length == 1 && value[0] == Docs.Null)
            {
                return;
            }
            this.AddRange(value);
        }
    }
}
