using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBlockSyntax(BlockSyntax node)
        {
            return this.PrintBlockSyntax(node, null);
        }

        private Doc PrintBlockSyntaxWithConditionalSpace(
            BlockSyntax node,
            string groupId
        ) {
            return this.PrintBlockSyntax(node, groupId);
        }

        // TODO this should really be private so it can't be used by anything but the two methods above
        private Doc PrintBlockSyntax(BlockSyntax node, string? groupId)
        {
            Doc statementSeparator = node.Parent is AccessorDeclarationSyntax
                && node.Statements.Count <= 1
                ? Docs.Line
                : Docs.HardLine;

            var docs = new List<Doc>
            {
                groupId != null
                    ? Docs.IfBreak(" ", Docs.Line, groupId)
                    : Docs.Line
            };
            SyntaxTokens.Print(node.OpenBraceToken, docs);

            if (node.Statements.Count > 0)
            {
                var innerDoc = Docs.Indent(
                    statementSeparator,
                    Join(statementSeparator, node.Statements.Select(this.Print))
                );

                DocUtilities.RemoveInitialDoubleHardLine(innerDoc);

                docs.Add(innerDoc, statementSeparator);
            }

            if (node.Statements.Count == 0)
            {
                docs.Add(" ");
            }

            SyntaxTokens.Print(node.CloseBraceToken, docs);

            return Docs.Group(docs);
        }
    }
}
