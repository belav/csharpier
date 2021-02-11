using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO Trivia here
        private Doc PrintElseClauseSyntax(ElseClauseSyntax node)
        {
            var parts = new Parts(this.PrintSyntaxToken(node.ElseKeyword));
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else if (node.Statement is IfStatementSyntax)
            {
                parts.Push(" ", statement);
            }
            else
            {
                parts.Push(HardLine, "{", Indent(Concat(HardLine, statement)), HardLine, "}");
            }

            return Concat(parts);
        }
    }
}