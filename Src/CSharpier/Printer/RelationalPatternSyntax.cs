using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRelationalPatternSyntax(RelationalPatternSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OperatorToken),
                " ",
                this.Print(node.Expression)
            );
        }
    }
}
