using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingStatementSyntax(UsingStatementSyntax node)
        {
            var groupId = GroupIdGenerator.GenerateGroupIdFor(node);

            var parts = new Parts(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.AwaitKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.UsingKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.OpenParenToken),
                Docs.GroupWithId(
                    groupId,
                    node.Declaration != null
                        ? this.PrintVariableDeclarationSyntax(node.Declaration)
                        : Doc.Null,
                    node.Expression != null
                        ? this.Print(node.Expression)
                        : Doc.Null,
                    Docs.SoftLine
                ),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
            var statement = this.Print(node.Statement);
            if (node.Statement is UsingStatementSyntax)
            {
                parts.Push(HardLine, statement);
            }
            else if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else
            {
                parts.Push(Indent(Concat(HardLine, statement)));
            }

            GroupIdGenerator.RemoveGroupIdFor(node);

            return Concat(parts);
        }
    }
}
