using System.Collections.Generic;

namespace CSharpier
{
    public class Doc
    {
        public static implicit operator Doc(string value)
        {
            return new StringDoc(value);
        }
    }
    
    public class IndentDoc : Doc
    {
        public Doc Contents { get; set; }
    }
    
    public class StringDoc : Doc
    {
        public string Value { get; set; }
        
        public StringDoc(string value)
        {
            this.Value = value;
        }
    }
    
    public class LineDoc : Doc
    {
        public enum LineType
        {
            Normal,
            Hard,
            Soft
        }
        
        public LineType Type { get; set; }
        public bool IsLiteral { get; set; }
    }

    public class Group : Doc
    {
        public Doc Contents { get; set; }
        public bool Break { get; set; }
    }
    
    public class BreakParent : Doc
    {
    }

    public class Concat : Doc
    {
        public List<Doc> Contents { get; set; }
    }
}