using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class WhileStatement
    {
        public static Doc Print(WhileStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var result = Doc.Concat(
                ExtraNewLines.Print(node),
                Token.PrintWithSuffix(node.WhileKeyword, " "),
                Token.Print(node.OpenParenToken),
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Condition)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken),
                node.Statement is BlockSyntax blockSyntax
                    ? Block.PrintWithConditionalSpace(blockSyntax, groupId)
                    : Node.Print(node.Statement)
            );

            return result;
        }
    }
}
