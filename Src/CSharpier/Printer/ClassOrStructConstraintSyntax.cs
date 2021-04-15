using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintClassOrStructConstraintSyntax(
            ClassOrStructConstraintSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.ClassOrStructKeyword),
                SyntaxTokens.Print(node.QuestionToken)
            );
        }
    }
}
