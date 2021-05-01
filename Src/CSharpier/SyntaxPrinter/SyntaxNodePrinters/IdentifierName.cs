using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class IdentifierName
    {
        public static Doc Print(IdentifierNameSyntax node)
        {
            return Token.Print(node.Identifier);
        }
    }
}
