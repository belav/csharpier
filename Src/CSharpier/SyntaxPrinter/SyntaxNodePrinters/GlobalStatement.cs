using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class GlobalStatement
    {
        public static Doc Print(GlobalStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                new Printer().PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Node.Print(node.Statement)
            );
        }
    }
}
