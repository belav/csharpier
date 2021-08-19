using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class LockStatement
    {
        public static Doc Print(LockStatementSyntax node)
        {
            var statement = Node.Print(node.Statement);

            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.PrintWithSuffix(node.LockKeyword, " "),
                Token.Print(node.OpenParenToken),
                Node.Print(node.Expression),
                Token.Print(node.CloseParenToken),
                node.Statement is BlockSyntax ? statement : Doc.Indent(Doc.HardLine, statement)
            );
        }
    }
}
