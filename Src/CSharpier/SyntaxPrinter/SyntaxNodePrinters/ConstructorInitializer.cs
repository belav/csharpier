using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConstructorInitializer
    {
        public static Doc Print(ConstructorInitializerSyntax node)
        {
            return Doc.Concat(
                " ",
                Token.PrintWithSuffix(node.ColonToken, " "),
                Token.Print(node.ThisOrBaseKeyword),
                ArgumentList.Print(node.ArgumentList)
            );
        }
    }
}
