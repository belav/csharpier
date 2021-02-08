using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
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
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            if (node.Members.Count > 0) {
                parts.Add(Join(HardLine, node.Members.Select(this.Print)));
            }

            this.PrintLeadingTrivia(node.EndOfFileToken.LeadingTrivia, parts);
            
            if (parts[^1] != HardLine) {
                parts.Add(HardLine);
            }
            return Concat(parts);
        }
    }
}
