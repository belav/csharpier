using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLocalDeclarationStatementSyntax(LocalDeclarationStatementSyntax node)
        {
            var parts = new Parts();
            var printedExtraNewLines = false;
            if (node.AwaitKeyword.RawKind != 0) {
                this.PrintLeadingTrivia(node.AwaitKeyword.LeadingTrivia, parts, ref printedExtraNewLines);
                parts.Add("await ");
            }
            if (node.UsingKeyword.RawKind != 0) {
                this.PrintLeadingTrivia(node.UsingKeyword.LeadingTrivia, parts, ref printedExtraNewLines);
                parts.Add("using ");
            }
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(this.PrintVariableDeclarationSyntax(node.Declaration));
            parts.Add(";");
            this.PrintTrailingTrivia(node.SemicolonToken.TrailingTrivia, parts);
            return Concat(parts);
        }
    }
}
