using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGenericNameSyntax(GenericNameSyntax node)
        {
            return Docs.Group(
                this.PrintSyntaxToken(node.Identifier),
                this.Print(node.TypeArgumentList)
            );
        }
    }
}
