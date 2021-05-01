using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAliasQualifiedNameSyntax(
            AliasQualifiedNameSyntax node
        ) {
            return Doc.Concat(
                this.Print(node.Alias),
                Token.Print(node.ColonColonToken),
                this.Print(node.Name)
            );
        }
    }
}
