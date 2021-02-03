using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintContinueStatementSyntax(ContinueStatementSyntax node)
        {
            return Concat(String(node.ContinueKeyword.Text), String(";"));
        }
    }
}
