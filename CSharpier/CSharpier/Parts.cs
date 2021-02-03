using System.Collections.Generic;
using System.Linq;

namespace CSharpier
{
    public class Parts
    {
        public readonly List<Doc> TheParts = new List<Doc>();

        public Parts()
        {
        }

        public Parts(params Doc[] parts)
        {
            this.TheParts = parts.ToList();
        }

        public void Push(params string[] values)
        {
            foreach (var value in values)
            {
                this.TheParts.Add(new StringDoc(value));
            }
        }
        
        public void Push(params Doc[] value)
        {
            this.TheParts.AddRange(value);
        }
        
        
    }
}