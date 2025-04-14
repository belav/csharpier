using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class FromClause
{
    public static Doc Print(FromClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.FromKeyword, " ", context),
            node.Type != null ? Doc.Concat(Node.Print(node.Type, context), " ") : Doc.Null,
            Token.PrintWithSuffix(node.Identifier, " ", context),
            Token.PrintWithSuffix(node.InKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
