using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class IdentifierName
    {
        public static Doc Print(IdentifierNameSyntax node)
        {
            return Token.Print(node.Identifier);
        }
    }
}
