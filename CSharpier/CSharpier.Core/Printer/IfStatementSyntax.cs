using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 1 the conditions in if statement should be formatted!
        private Doc PrintIfStatementSyntax(IfStatementSyntax node)
        {
            var parts = new Parts();
            if (!(node.Parent is ElseClauseSyntax))
            {
                 parts.Push(this.PrintExtraNewLines(node));
            }
            parts.Push(this.PrintSyntaxToken(node.IfKeyword, " "),
                this.PrintSyntaxToken(node.OpenParenToken),
                this.Print(node.Condition),
                this.PrintSyntaxToken(node.CloseParenToken));
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else
            {
                parts.Push(HardLine, "{", Indent(Concat(HardLine, statement)), HardLine, "}");
            }

            if (node.Else != null)
            {
                parts.Push(HardLine, this.Print(node.Else));
            }

            return Concat(parts);
        }
    }
}