using System.Collections.Generic;
using System.Linq;

namespace CSharpier
{
    public class Parts : List<Doc>
    {
        public Parts()
        {
        }

        public Parts(params Doc[] parts)
        {
            this.AddRange(parts);
        }

        public void Push(params Doc[] value)
        {
            this.AddRange(value);
        }
    }
}