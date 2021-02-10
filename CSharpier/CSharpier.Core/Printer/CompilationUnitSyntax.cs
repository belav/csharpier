using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCompilationUnitSyntax(CompilationUnitSyntax node)
        {
            var parts = new Parts();
            if (node.Externs.Count > 0) {
                parts.Push(
                    Join(
                        HardLine,
                        node.Externs.Select(this.PrintExternAliasDirectiveSyntax)
                    ),
                    HardLine
                );
            }
            if (node.Usings.Count > 0) {
                parts.Push(
                    Join(
                        HardLine,
                        node.Usings.Select(this.PrintUsingDirectiveSyntax)
                    ),
                    HardLine
                );
            }
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            if (node.Members.Count > 0) {
                parts.Add(Join(HardLine, node.Members.Select(this.Print)));
            }

            var finalTrivia = this.PrintLeadingTrivia(node.EndOfFileToken.LeadingTrivia);
            if (finalTrivia != null)
            {
                parts.Push(finalTrivia);
            }
            
            if (parts.Count == 0 || (parts[^1] != HardLine && parts[^1] is Concat concat && concat.Parts[^1] != HardLine)) {
                parts.Add(HardLine);
            }
            return Concat(parts);
        }
    }
}
