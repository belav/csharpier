using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class UsingDirective
    {
        public static Doc Print(UsingDirectiveSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.PrintWithSuffix(node.UsingKeyword, " "),
                Token.PrintWithSuffix(node.StaticKeyword, " "),
                node.Alias == null ? Doc.Null : NameEquals.Print(node.Alias),
                Node.Print(node.Name),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
