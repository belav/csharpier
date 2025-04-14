using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ClassOrStructConstraint
{
    public static Doc Print(ClassOrStructConstraintSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.ClassOrStructKeyword, context),
            Token.Print(node.QuestionToken, context)
        );
    }
}
