using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RefType
    {
        public static Doc Print(RefTypeSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.RefKeyword, " "),
                Token.Print(node.ReadOnlyKeyword, " "),
                Node.Print(node.Type)
            );
        }
    }
}
