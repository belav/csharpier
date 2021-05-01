using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class LocalFunctionStatement
    {
        public static Doc Print(LocalFunctionStatementSyntax node)
        {
            return BaseMethodDeclaration.Print(node);
        }
    }
}
