using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrowExpressionClause
{
    public static Doc Print(ArrowExpressionClauseSyntax node, CSharpPrintingContext context)
    {
        if (Token.IsMultiLineRawString(node.Expression))
        {
            return Doc.Concat(
                " ",
                Token.PrintWithSuffix(node.ArrowToken, " ", context),
                Node.Print(node.Expression, context)
            );
        }

        return Doc.Group(
            Doc.Indent(
                " ",
                Token.PrintWithSuffix(node.ArrowToken, Doc.Line, context),
                Node.Print(node.Expression, context)
            )
        );
    }
}
