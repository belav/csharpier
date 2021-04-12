using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVarPatternSyntax(VarPatternSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.VarKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Designation)
            );
        }
    }
}
