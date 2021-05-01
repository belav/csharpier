using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var result = Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.WhileKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                Token.Print(node.OpenParenToken),
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(Doc.SoftLine, this.Print(node.Condition)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken),
                node.Statement is BlockSyntax blockSyntax
                    ? this.PrintBlockSyntaxWithConditionalSpace(
                            blockSyntax,
                            groupId
                        )
                    : this.Print(node.Statement)
            );

            return result;
        }
    }
}
