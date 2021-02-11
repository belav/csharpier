using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintClassOrStructConstraintSyntax(ClassOrStructConstraintSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.ClassOrStructKeyword), this.PrintSyntaxToken(node.QuestionToken));
        }
    }
}
