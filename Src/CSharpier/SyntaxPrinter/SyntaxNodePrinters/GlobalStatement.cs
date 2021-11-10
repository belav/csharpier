using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class GlobalStatement
    {
        public static Doc Print(GlobalStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                AttributeLists.Print(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Node.Print(node.Statement)
            );
        }
    }
}
