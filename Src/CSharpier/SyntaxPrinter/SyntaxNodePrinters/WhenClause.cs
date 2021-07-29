using System;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class WhenClause
    {
        public static Doc Print(WhenClauseSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();
            return Doc.GroupWithId(
                groupId,
                Doc.Indent(
                    Doc.Line,
                    Token.PrintWithSuffix(node.WhenKeyword, " "),
                    Node.Print(node.Condition)
                )
            );
        }
    }
}
