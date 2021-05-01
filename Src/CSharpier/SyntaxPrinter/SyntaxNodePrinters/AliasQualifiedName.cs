using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AliasQualifiedName
    {
        public static Doc Print(AliasQualifiedNameSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Alias),
                Token.Print(node.ColonColonToken),
                Node.Print(node.Name)
            );
        }
    }
}
