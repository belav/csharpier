using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class LetClause
{
    public static Doc Print(LetClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.LetKeyword, " ", context),
            Token.PrintWithSuffix(node.Identifier, " ", context),
            Token.PrintWithSuffix(node.EqualsToken, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
