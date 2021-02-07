using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintContinueStatementSyntax(ContinueStatementSyntax node)
        {
            // TODO optimization review all SyntaxToken.Text? what about printing comments before/after SyntaxTokens?
            // TODO optimization get rid of String() where not needed
            return Concat(String(node.ContinueKeyword.Text), String(";"));
        }
    }
}
