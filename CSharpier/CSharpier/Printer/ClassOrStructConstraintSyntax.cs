using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintClassOrStructConstraintSyntax(ClassOrStructConstraintSyntax node)
        {
            return Concat(String(node.ClassOrStructKeyword.Text), String(node.QuestionToken.Text));
        }
    }
}
