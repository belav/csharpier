using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLocalDeclarationStatementSyntax(
            LocalDeclarationStatementSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.AwaitKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.UsingKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintModifiers(node.Modifiers),
                this.PrintVariableDeclarationSyntax(node.Declaration),
                this.PrintSyntaxToken(node.SemicolonToken)
            );
        }
    }
}
