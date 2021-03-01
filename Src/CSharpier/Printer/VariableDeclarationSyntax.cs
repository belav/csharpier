using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclarationSyntax(
            VariableDeclarationSyntax node)
        {

            return Concat(
                this.Print(node.Type),
                SpaceIfNoPreviousComment,
                //Indent(
                this.PrintSeparatedSyntaxList(
                    node.Variables,
                    this.PrintVariableDeclaratorSyntax,
                    " ")
                //)
                
                );
        }
    }
}
