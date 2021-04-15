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
            return Docs.Concat(
                this.Print(node.Alias),
                SyntaxTokens.Print(node.ColonColonToken),
                this.Print(node.Name)
            );
        }
    }
}
