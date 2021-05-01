using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ForEachVariableStatement
    {
        public static Doc Print(ForEachVariableStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.AwaitKeyword, " "),
                Token.Print(node.ForEachKeyword, " "),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Variable),
                " ",
                Token.Print(node.InKeyword, " "),
                Node.Print(node.Expression),
                Token.Print(node.CloseParenToken),
                Node.Print(node.Statement)
            );
        }
    }
}
