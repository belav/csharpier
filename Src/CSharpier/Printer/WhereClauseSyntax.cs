using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
        {
            return Docs.Group(
                this.PrintSyntaxToken(node.WhereKeyword),
                Docs.Indent(Docs.Line, this.Print(node.Condition))
            );
        }
    }
}
