using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElseClause
{
    public static Doc Print(ElseClauseSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.ElseKeyword),
            node.Statement is IfStatementSyntax ifStatementSyntax
              ? Doc.Concat(" ", IfStatement.Print(ifStatementSyntax))
              : OptionalBraces.Print(node.Statement)
        );
    }
}
