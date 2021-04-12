using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGlobalStatementSyntax(GlobalStatementSyntax node)
        {
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers),
                this.Print(node.Statement)
            );
        }
    }
}
