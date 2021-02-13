using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintVariableDeclarationSyntax(VariableDeclarationSyntax node)
        {
            return Concat(this.Print(node.Type),
                SpaceIfNoPreviousComment,
                this.PrintSeparatedSyntaxList(node.Variables, this.PrintVariableDeclaratorSyntax, " "));
        }
    }
}
