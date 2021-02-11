using System.Collections.Generic;

namespace CSharpier.Core
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
            if (value.Length == 1 && value[0] == null)
            {
                return;
            }
            this.AddRange(value);
        }
    }
}