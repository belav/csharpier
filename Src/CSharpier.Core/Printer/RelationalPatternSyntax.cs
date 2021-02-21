using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintRelationalPatternSyntax(RelationalPatternSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.OperatorToken),
                " ",
                this.Print(node.Expression));
        }
    }
}