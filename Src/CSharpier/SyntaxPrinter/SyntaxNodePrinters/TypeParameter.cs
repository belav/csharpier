using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TypeParameter
    {
        public static Doc Print(TypeParameterSyntax node)
        {
            var hasAttribute = node.AttributeLists.Any();
            var groupId = hasAttribute ? Guid.NewGuid().ToString() : string.Empty;

            var result = Doc.Concat(
                AttributeLists.Print(node, node.AttributeLists),
                hasAttribute ? Doc.IndentIfBreak(Doc.Line, groupId) : Doc.Null,
                node.VarianceKeyword.Kind() != SyntaxKind.None
                  ? Token.PrintWithSuffix(node.VarianceKeyword, " ")
                  : Doc.Null,
                Token.Print(node.Identifier)
            );

            return hasAttribute ? Doc.GroupWithId(groupId, result) : result;
        }
    }
}
