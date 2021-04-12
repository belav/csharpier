using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRelationalPatternSyntax(RelationalPatternSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(node.OperatorToken),
                " ",
                this.Print(node.Expression)
            );
        }
    }
}
