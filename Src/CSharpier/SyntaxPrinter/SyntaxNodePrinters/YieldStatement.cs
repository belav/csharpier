using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class YieldStatement
{
    public static Doc Print(YieldStatementSyntax node)
    {
        Doc expression =
            node.Expression != null ? Doc.Concat(" ", Node.Print(node.Expression)) : string.Empty;
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.YieldKeyword, " "),
            Token.Print(node.ReturnOrBreakKeyword),
            expression,
            Token.Print(node.SemicolonToken)
        );
    }
}
