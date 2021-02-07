using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO this happened with invalid c#, and then formatted wrong, can we not format on invalid c#??
        /*
public class ClassName()
{
}
         */
        
        private Doc PrintGlobalStatementSyntax(GlobalStatementSyntax node)
        {
            var parts = new Parts();
            var printedExtraNewLines = false;
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            parts.Push(this.PrintModifiers(node.Modifiers, ref printedExtraNewLines));
            parts.Push(this.Print(node.Statement));
            return Concat(parts);
        }
    }
}
