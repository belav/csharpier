using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class EmptyStatement
{
    public static Doc Print(EmptyStatementSyntax node, PrintingContext context)
    {
        return Token.Print(node.SemicolonToken, context);
    }
}
