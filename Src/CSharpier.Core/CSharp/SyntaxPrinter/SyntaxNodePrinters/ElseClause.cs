using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElseClause
{
    public static Doc Print(ElseClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.ElseKeyword, context),
            node.Statement is IfStatementSyntax ifStatementSyntax
                ? Doc.Concat(" ", IfStatement.Print(ifStatementSyntax, context))
                : OptionalBraces.Print(node.Statement, context)
        );
    }
}
