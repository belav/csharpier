using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class UnaryPattern
{
    public static Doc Print(UnaryPatternSyntax node, CSharpPrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.OperatorToken, " ", context),
            Node.Print(node.Pattern, context)
        );
    }
}
