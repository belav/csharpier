using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 0 separatedSyntaxList has commaTokens, where else are they??
        private Doc PrintParameterListSyntax(ParameterListSyntax node)
        {
            var parts = new Parts();
            for (var x = 0; x < node.Parameters.Count; x++)
            {
                // TODO 0 we need some way to break the group if there is leading/trailing in the middle
                /*
    // tester
    public static class ClassName { } // that shouldn't break
    
    void MethodName(
        string one, // this should break, to force parameters to line up, it doesn't currently
        string two,
        string three) { }
                 */
                parts.Push(this.PrintParameterSyntax(node.Parameters[x]));
                if (x < node.Parameters.Count - 1)
                {
                    parts.Push(this.PrintSyntaxToken(node.Parameters.GetSeparator(x), Line));
                }
            }
            return Group(this.PrintSyntaxToken(node.OpenParenToken), 
                parts.Count > 0 ? Indent(SoftLine, Concat(parts)) : null,
                this.PrintSyntaxToken(node.CloseParenToken));
        }
    }
}
