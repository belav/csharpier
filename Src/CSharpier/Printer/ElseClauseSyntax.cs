using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
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
                // TODO 1 force braces here?
                parts.Push(Indent(Concat(HardLine, statement)));
            }

            return Concat(parts);
        }
    }
}
