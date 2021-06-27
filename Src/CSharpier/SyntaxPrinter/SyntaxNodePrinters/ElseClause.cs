using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ElseClause
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
}
