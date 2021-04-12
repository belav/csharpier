using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNullableTypeSyntax(NullableTypeSyntax node)
        {
            return Docs.Concat(
                this.Print(node.ElementType),
                this.PrintSyntaxToken(node.QuestionToken)
            );
        }
    }
}
