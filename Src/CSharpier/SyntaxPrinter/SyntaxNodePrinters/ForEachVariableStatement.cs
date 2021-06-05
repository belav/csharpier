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
                Token.PrintWithSuffix(node.AwaitKeyword, " "),
                Token.PrintWithSuffix(node.ForEachKeyword, " "),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Variable),
                " ",
                Token.PrintWithSuffix(node.InKeyword, " "),
                Node.Print(node.Expression),
                Token.Print(node.CloseParenToken),
                Node.Print(node.Statement)
            );
        }
    }
}
