using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
        {
            return Concat("where ", this.Print(node.Condition));
        }
    }
}
