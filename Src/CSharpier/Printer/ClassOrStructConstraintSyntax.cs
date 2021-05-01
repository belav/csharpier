using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintClassOrStructConstraintSyntax(
            ClassOrStructConstraintSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.ClassOrStructKeyword),
                Token.Print(node.QuestionToken)
            );
        }
    }
}
