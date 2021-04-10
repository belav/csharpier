using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForEachStatementSyntax(ForEachStatementSyntax node)
        {
            var groupId = GroupIdGenerator.GenerateGroupIdFor(node);

            var result = Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.AwaitKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.ForEachKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Type),
                " ",
                this.PrintSyntaxToken(
                    node.Identifier,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.InKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                Docs.GroupWithId(
                    groupId,
                    this.Print(node.Expression),
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
