using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class QualifiedName
    {
        public static Doc Print(QualifiedNameSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Left),
                Token.Print(node.DotToken),
                Node.Print(node.Right)
            );
        }
    }
}
