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
    
    public class IndentDoc : Doc, IHasContents
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

    public class Group : Doc, IHasContents
    {
        public Doc Contents { get; set; }
        public bool Break { get; set; }
        public bool ExpandedStates { get; set; }
    }
    
    public class BreakParent : Doc
    {
    }

    public class Concat : Doc
    {
        public List<Doc> Parts { get; set; }
    }

    public class Align : Doc, IHasContents
    {
        public Doc Contents { get; set; }
    }
    
    public class Fill : Doc, IHasContents
    {
        public Doc Contents { get; set; }
    }
    
    public class LineSuffix : Doc, IHasContents
    {
        public Doc Contents { get; set; }
    }

    interface IHasContents
    {
        Doc Contents { get; set; }
    }
}