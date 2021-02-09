using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLocalDeclarationStatementSyntax(LocalDeclarationStatementSyntax node)
        {
            var parts = new Parts();
            this.printNewLinesInLeadingTrivia.Push(true);
            if (node.AwaitKeyword.RawKind != 0) {
                parts.Push(this.PrintLeadingTrivia(node.AwaitKeyword));
                parts.Add("await ");
            }
            if (node.UsingKeyword.RawKind != 0) {
                parts.Push(this.PrintLeadingTrivia(node.UsingKeyword));
                parts.Add("using ");
            }
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(this.PrintVariableDeclarationSyntax(node.Declaration));
            this.printNewLinesInLeadingTrivia.Pop();
            parts.Add(";");
            parts.Push(this.PrintTrailingTrivia(node.SemicolonToken));
            return Concat(parts);
        }
    }
}
