using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintThisExpressionSyntax(ThisExpressionSyntax node)
        {
            return node.Token.Text;
        }
    }
}
