using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO 1 this breaks weird
        // return $"Order jlkasdf saldkfj alsdkfj alksdfasdf is incorrect: [{linksText[i]}]";
        private Doc PrintBracketedArgumentListSyntax(
            BracketedArgumentListSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.OpenBracketToken),
                Indent(
                    Concat(
                        SoftLine,
                        this.PrintSeparatedSyntaxList(
                            node.Arguments,
                            this.Print,
                            Line))),
                SoftLine,
                this.PrintSyntaxToken(node.CloseBracketToken));
        }
    }
}
