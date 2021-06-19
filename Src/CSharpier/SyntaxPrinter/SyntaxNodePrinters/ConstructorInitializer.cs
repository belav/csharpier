using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConstructorInitializer
    {
        public static Doc PrintWithConditionalSpace(
            ConstructorInitializerSyntax node,
            string groupId
        ) {
            return Print(node, groupId);
        }

        public static Doc Print(ConstructorInitializerSyntax node)
        {
            return Print(node, null);
        }

        private static Doc Print(ConstructorInitializerSyntax node, string? groupId)
        {
            var colonToken = Token.PrintWithSuffix(node.ColonToken, " ");
            return Doc.Group(
                Doc.Indent(
                    groupId != null ? Doc.IfGroupBreaks(" ", groupId).Else(Doc.Line) : Doc.Line
                ),
                groupId != null
                    ? Doc.IfGroupBreaks(Doc.Align(2, colonToken), groupId)
                            .Else(Doc.Indent(colonToken))
                    : Doc.Indent(colonToken),
                Token.Print(node.ThisOrBaseKeyword),
                groupId != null
                    ? ArgumentList.Print(node.ArgumentList)
                    : Doc.Indent(ArgumentList.Print(node.ArgumentList))
            );
        }
    }
}
