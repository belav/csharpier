using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBlockSyntax(BlockSyntax node)
        {
            if (node == null) // TODO 1 why is this being called?
            {
                return "";
            }
            var statementSeparator = node.Parent is AccessorDeclarationSyntax && node.Statements.Count <= 1 ? Line : HardLine;
            return this.PrintStatements(node.OpenBraceToken, node.Statements, node.CloseBraceToken, statementSeparator);
        }
    }
}