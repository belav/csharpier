using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBaseListSyntax(BaseListSyntax node)
        {
            /* TODO this should format like this instead
            : AnotherLongClassName<T>,
                AndYetAnotherLongClassName
            void MethodName() { }
            */
            return Group(Indent(Concat(Line, ":", " ", this.PrintCommaList(node.Types.Select(this.Print)))));
        }
    }
}
