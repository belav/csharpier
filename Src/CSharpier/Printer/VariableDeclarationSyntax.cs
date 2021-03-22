using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclarationSyntax(
            VariableDeclarationSyntax node)
        {
            var innerParts = Concat(
                this.PrintSeparatedSyntaxList(
                    node.Variables,
                    this.PrintVariableDeclaratorSyntax,
                    node.Parent is ForStatementSyntax ? Line : HardLine
                )
            );

            return Concat(
                this.Print(node.Type),
                SpaceIfNoPreviousComment,
                node.Variables.Count > 1 ? Indent(innerParts) : innerParts
            );
        }
    }
}
