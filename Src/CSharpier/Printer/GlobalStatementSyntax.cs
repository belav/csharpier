using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGlobalStatementSyntax(GlobalStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                this.Print(node.Statement)
            );
        }
    }
}
