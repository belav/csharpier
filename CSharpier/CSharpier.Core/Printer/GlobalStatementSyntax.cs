using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
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
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.Print(node.Statement));
            return Concat(parts);
        }
    }
}
