using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class FixedStatement
    {
        public static Doc Print(FixedStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.FixedKeyword, " "),
                Token.Print(node.OpenParenToken),
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Declaration)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken),
                node.Statement is BlockSyntax blockSyntax
                    ? Block.PrintWithConditionalSpace(blockSyntax, groupId)
                    : Node.Print(node.Statement)
            );
        }
    }
}
