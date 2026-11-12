using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class YieldStatement
{
    public static Doc Print(YieldStatementSyntax node, CSharpPrintingContext context)
    {
        var expression =
            node.Expression != null
                ? Doc.Concat(" ", Node.Print(node.Expression, context))
                : Doc.Null;
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.YieldKeyword, " ", context),
            Token.Print(node.ReturnOrBreakKeyword, context),
            expression,
            Token.Print(node.SemicolonToken, context)
        );
    }
}
