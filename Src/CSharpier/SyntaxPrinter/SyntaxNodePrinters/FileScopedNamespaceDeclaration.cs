using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class FileScopedNamespaceDeclaration
    {
        public static Doc Print(FileScopedNamespaceDeclarationSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                AttributeLists.Print(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.NamespaceKeyword),
                " ",
                Node.Print(node.Name),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
