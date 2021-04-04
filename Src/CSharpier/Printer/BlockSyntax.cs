using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBlockSyntax(BlockSyntax node)
        {
            if (node == null) // TODO 1 why is this being called?
            {
                return Doc.Null;
            }

            var statementSeparator = node.Parent is AccessorDeclarationSyntax
                && node.Statements.Count <= 1
                ? Line
                : HardLine;

            var parts = new List<Doc>
            {
                Line,
                this.PrintSyntaxToken(node.OpenBraceToken)
            };
            if (node.Statements.Count > 0)
            {
                var innerParts = Indent(
                    statementSeparator,
                    Join(statementSeparator, node.Statements.Select(this.Print))
                );

                DocUtilities.RemoveInitialDoubleHardLine(innerParts);

                parts.Add(Concat(innerParts, statementSeparator));
            }

            parts.Add(
                this.PrintSyntaxToken(
                    node.CloseBraceToken,
                    null,
                    node.Statements.Count == 0 ? " " : Doc.Null
                )
            );
            return Group(parts);
        }
    }
}
