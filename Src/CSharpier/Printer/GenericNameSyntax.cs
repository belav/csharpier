using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintGenericNameSyntax(GenericNameSyntax node)
        {
            return Doc.Group(
                Token.Print(node.Identifier),
                this.Print(node.TypeArgumentList)
            );
        }
    }
}
