using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SingleVariableDesignation
    {
        public static Doc Print(SingleVariableDesignationSyntax node)
        {
            return Token.Print(node.Identifier);
        }
    }
}
