using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLocalDeclarationStatementSyntax(LocalDeclarationStatementSyntax node)
        {
            var parts = new Parts();
            // printExtraNewLines(
            //     node,
            //     parts,
            //     String("awaitKeyword"),
            //     String("usingKeyword"),
            //     String("modifiers"),
            //     [String("declaration"), String("type"), String("keyword")],
            //     [String("declaration"), String("type"), String("identifier")]
            // );
            // printLeadingComments(
            //     node,
            //     parts,
            //     String("awaitKeyword"),
            //     String("usingKeyword"),
            //     String("modifiers"),
            //     [String("declaration"), String("type"), String("keyword")],
            //     [String("declaration"), String("type"), String("identifier")]
            // );
            if (node.AwaitKeyword.RawKind != 0) {
                parts.Add(String("await "));
            }
            if (node.UsingKeyword.RawKind != 0) {
                parts.Add(String("using "));
            }
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(this.PrintVariableDeclarationSyntax(node.Declaration));
            parts.Add(String(";"));
            // TODO printTrailingComments(node, parts, String("semicolonToken"));
            return Concat(parts);
        }
    }
}
