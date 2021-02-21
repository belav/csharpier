using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.WhereKeyword, " "),
                this.Print(node.Condition));
        }
    }
}