using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class LocalFunctionStatement
{
    public static Doc Print(LocalFunctionStatementSyntax node, PrintingContext context)
    {
        return BaseMethodDeclaration.Print(node, context);
    }
}
