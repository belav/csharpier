using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBaseListSyntax(BaseListSyntax node)
        {
            /* TODO 1 this should format like this instead
            : AnotherLongClassName<T>,
                AndYetAnotherLongClassName
            void MethodName() { }
            */
            return Group(Indent(Line,
                this.PrintSyntaxToken(node.ColonToken, " "),
                this.PrintSeparatedSyntaxList(node.Types, this.Print, Line)
            ));
        }
    }
}