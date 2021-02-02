using System.Collections.Generic;
using System.Linq;

namespace CSharpier
{
    public class Parts
    {
        public readonly List<Builder> TheParts = new List<Builder>();

        public Parts()
        {
        }
        
        public Parts(Builder[] parts)
        {
            this.TheParts = parts.ToList();
        }

        public void Push(params string[] values)
        {
            foreach (var value in values)
            {
                this.TheParts.Add(Builders.String(value));
            }
        }
        
        public void Push(params Builder[] value)
        {
            this.TheParts.AddRange(value);
        }
        
        
    }
}