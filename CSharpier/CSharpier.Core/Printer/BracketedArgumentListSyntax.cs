using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBracketedArgumentListSyntax(BracketedArgumentListSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.OpenBracketToken),
                Indent(Concat(SoftLine, this.PrintCommaList(node.Arguments.Select(this.Print)))),
                SoftLine, 
                this.PrintSyntaxToken(node.CloseBracketToken)
            );
        }
    }
}
