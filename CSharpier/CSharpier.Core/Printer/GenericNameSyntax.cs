using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintGenericNameSyntax(GenericNameSyntax node)
        {
            return Group(Concat(node.Identifier.Text, "<", this.Print(node.TypeArgumentList), ">"));
        }
    }
}
