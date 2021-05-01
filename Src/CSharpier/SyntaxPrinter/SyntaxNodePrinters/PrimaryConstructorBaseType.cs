using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class PrimaryConstructorBaseType
    {
        public static Doc Print(PrimaryConstructorBaseTypeSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Type),
                ArgumentList.Print(node.ArgumentList)
            );
        }
    }
}
