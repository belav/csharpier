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
            return Doc.Group(
                Doc.Indent(groupId != null ? Doc.IfBreak(" ", Doc.Line, groupId) : Doc.Line),
                Token.PrintWithSuffix(node.ColonToken, " "),
                Token.Print(node.ThisOrBaseKeyword),
                groupId != null ? ArgumentList.Print(node.ArgumentList) : Doc.Indent(ArgumentList.Print(node.ArgumentList))
            );
        }
    }
}
