using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVarPatternSyntax(VarPatternSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.VarKeyword, " "),
                this.Print(node.Designation)
            );
        }
    }
}
