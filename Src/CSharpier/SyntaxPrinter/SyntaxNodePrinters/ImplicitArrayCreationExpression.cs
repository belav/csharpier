using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ImplicitArrayCreationExpression
    {
        public static Doc Print(ImplicitArrayCreationExpressionSyntax node)
        {
            var commas = node.Commas.Select(Token.Print).ToArray();
            return Doc.Concat(
                Token.Print(node.NewKeyword),
                Token.Print(node.OpenBracketToken),
                Doc.Concat(commas),
                Token.PrintWithSuffix(node.CloseBracketToken, " "),
                Node.Print(node.Initializer)
            );
        }
    }
}
