using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGenericNameSyntax(GenericNameSyntax node)
        {
            return Group(Concat(String(node.Identifier.Text), String("<"), this.Print(node.TypeArgumentList), String(">")));
        }
    }
}
