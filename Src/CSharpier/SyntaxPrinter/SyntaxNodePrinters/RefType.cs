using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class RefType
    {
        public static Doc Print(RefTypeSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.RefKeyword, " "),
                Token.PrintWithSuffix(node.ReadOnlyKeyword, " "),
                Node.Print(node.Type)
            );
        }
    }
}
