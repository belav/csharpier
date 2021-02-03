using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
        {
            return Concat(String("where "), this.Print(node.Condition));
        }
    }
}
