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
                this.PrintLeadingTrivia(node.AwaitKeyword.LeadingTrivia, parts);
                parts.Add("await ");
            }
            if (node.UsingKeyword.RawKind != 0) {
                this.PrintLeadingTrivia(node.UsingKeyword.LeadingTrivia, parts);
                parts.Add("using ");
            }
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(this.PrintVariableDeclarationSyntax(node.Declaration));
            this.printNewLinesInLeadingTrivia.Pop();
            parts.Add(";");
            this.PrintTrailingTrivia(node.SemicolonToken.TrailingTrivia, parts);
            return Concat(parts);
        }
    }
}
