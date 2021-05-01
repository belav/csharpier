using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNullableTypeSyntax(NullableTypeSyntax node)
        {
            return Doc.Concat(
                this.Print(node.ElementType),
                Token.Print(node.QuestionToken)
            );
        }
    }
}
