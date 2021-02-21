using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBlockSyntax(BlockSyntax node)
        {
            if (node == null) // TODO 1 why is this being called?
            {
                return null;
            }
            
            var statementSeparator = node.Parent is AccessorDeclarationSyntax && node.Statements.Count <= 1 
                ? Line : HardLine;
            
            var parts = new Parts(Line, this.PrintSyntaxToken(node.OpenBraceToken));
            if (node.Statements.Count > 0)
            {
                parts.Push(Concat(Indent(statementSeparator, Join(statementSeparator, node.Statements.Select(this.Print))), statementSeparator));
            }

            parts.Push(this.PrintSyntaxToken(node.CloseBraceToken, null, node.Statements.Count == 0 ? " " : null));
            return Group(Concat(parts));
        }
    }
}