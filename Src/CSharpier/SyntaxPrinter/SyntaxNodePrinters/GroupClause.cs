using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class GroupClause
    {
        public static Doc Print(GroupClauseSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.GroupKeyword, " "),
                Node.Print(node.GroupExpression),
                " ",
                Token.PrintWithSuffix(node.ByKeyword, " "),
                Node.Print(node.ByExpression)
            );
        }
    }
}
