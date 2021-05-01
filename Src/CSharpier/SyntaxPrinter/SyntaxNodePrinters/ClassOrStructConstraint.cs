using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ClassOrStructConstraint
    {
        public static Doc Print(ClassOrStructConstraintSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.ClassOrStructKeyword),
                Token.Print(node.QuestionToken)
            );
        }
    }
}
