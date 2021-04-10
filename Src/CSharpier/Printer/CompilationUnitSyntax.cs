using System.Linq;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCompilationUnitSyntax(CompilationUnitSyntax node)
        {
            var parts = new Parts();
            if (node.Externs.Count > 0)
            {
                parts.Push(
                    Join(
                        HardLine,
                        node.Externs.Select(
                            this.PrintExternAliasDirectiveSyntax
                        )
                    ),
                    HardLine
                );
            }
            if (node.Usings.Count > 0)
            {
                parts.Push(
                    Join(
                        HardLine,
                        node.Usings.Select(this.PrintUsingDirectiveSyntax)
                    ),
                    HardLine
                );
            }
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            if (node.Members.Count > 0)
            {
                parts.Push(Join(HardLine, node.Members.Select(this.Print)));
            }

            var finalTrivia = SyntaxTokens.PrintLeadingTrivia(
                node.EndOfFileToken.LeadingTrivia,
                includeInitialNewLines: true
            );
            if (finalTrivia != Doc.Null)
            {
                // even though we include the initialNewLines above, a literalLine from directives trims the hardline, so add an extra one here
                parts.Push(HardLine, finalTrivia);
            }

            return Concat(parts);
        }
    }
}
