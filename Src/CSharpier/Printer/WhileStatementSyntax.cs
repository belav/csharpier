using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            var groupId = GroupIdGenerator.GenerateGroupIdFor(node);

            var result = Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.WhileKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SyntaxTokens.Print(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    Docs.Indent(Docs.SoftLine, this.Print(node.Condition)),
                    Docs.SoftLine
                ),
                SyntaxTokens.Print(node.CloseParenToken),
                this.Print(node.Statement)
            );

            GroupIdGenerator.RemoveGroupIdFor(node);

            return result;
        }
    }
}
