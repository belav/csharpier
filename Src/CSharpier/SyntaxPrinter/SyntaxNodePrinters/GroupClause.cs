using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class GroupClause
    {
        public static Doc Print(GroupClauseSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.GroupKeyword, " "),
                Node.Print(node.GroupExpression),
                " ",
                Token.Print(node.ByKeyword, " "),
                Node.Print(node.ByExpression)
            );
        }
    }
}
