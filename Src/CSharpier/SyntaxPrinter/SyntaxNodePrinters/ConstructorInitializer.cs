using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConstructorInitializer
    {
        // TODO 0 where is this used? I changed it and nothing broke
        public static Doc Print(ConstructorInitializerSyntax node)
        {
            return Doc.Indent(
                Doc.HardLine,
                Token.Print(node.ColonToken, " "),
                Token.Print(node.ThisOrBaseKeyword),
                ArgumentList.Print(node.ArgumentList)
            );
        }
    }
}
