using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.WhereKeyword),
                Indent(Line, this.Print(node.Condition))
            );
        }
    }
}
