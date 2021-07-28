using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class OptionalBraces
    {
        public static Doc Print(StatementSyntax node, string? groupId = null)
        {
            return node is BlockSyntax blockSyntax
                ? groupId == null
                    ? Block.Print(blockSyntax)
                    : Block.PrintWithConditionalSpace(blockSyntax, groupId)
                : Doc.Indent(Doc.HardLine, Node.Print(node));
        }
    }
}
