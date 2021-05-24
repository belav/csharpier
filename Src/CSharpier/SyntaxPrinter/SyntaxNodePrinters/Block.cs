using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class Block
    {
        public static Doc Print(BlockSyntax node)
        {
            return Print(node, null);
        }

        public static Doc PrintWithConditionalSpace(BlockSyntax node, string groupId)
        {
            return Print(node, groupId);
        }

        private static Doc Print(BlockSyntax node, string? groupId)
        {
            Doc statementSeparator = node.Parent is AccessorDeclarationSyntax
            && node.Statements.Count <= 1 ? Doc.Line : Doc.HardLine;

            var docs = new List<Doc>
            {
                groupId != null ? Doc.IfBreak(" ", Doc.Line, groupId) : Doc.Line,
                Token.Print(node.OpenBraceToken)
            };
            if (node.Statements.Count > 0)
            {
                var innerDoc = Doc.Indent(
                    statementSeparator,
                    Doc.Join(statementSeparator, node.Statements.Select(Node.Print))
                );

                DocUtilities.RemoveInitialDoubleHardLine(innerDoc);

                docs.Add(Doc.Concat(innerDoc, statementSeparator));
            }

            docs.Add(
                node.Statements.Count == 0 ? " " : Doc.Null,
                Token.Print(node.CloseBraceToken)
            );
            return Doc.Group(docs);
        }
    }
}
