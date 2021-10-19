using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class WhenClause
    {
        public static Doc Print(WhenClauseSyntax node)
        {
            var content = new Doc[]
            {
                Doc.Line,
                Token.PrintWithSuffix(node.WhenKeyword, " "),
                Node.Print(node.Condition)
            };

            return Doc.Group(
                node.Parent is CasePatternSwitchLabelSyntax
                  ? Doc.Align(6, content)
                  : Doc.Indent(content)
            );
        }
    }
}
