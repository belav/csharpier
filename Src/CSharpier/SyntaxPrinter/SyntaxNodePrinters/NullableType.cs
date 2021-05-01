using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class NullableType
    {
        public static Doc Print(NullableTypeSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.ElementType),
                Token.Print(node.QuestionToken)
            );
        }
    }
}
