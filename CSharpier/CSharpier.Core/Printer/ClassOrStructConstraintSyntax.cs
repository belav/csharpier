using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintClassOrStructConstraintSyntax(ClassOrStructConstraintSyntax node)
        {
            return Concat(node.ClassOrStructKeyword.Text, node.QuestionToken.Text);
        }
    }
}
