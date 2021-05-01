using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIsPatternExpressionSyntax(
            IsPatternExpressionSyntax node
        ) {
            return Doc.Concat(
                Node.Print(node.Expression),
                Doc.Indent(
                    Doc.Line,
                    Token.PrintWithSuffix(node.IsKeyword, " "),
                    Doc.Indent(this.Print(node.Pattern))
                )
            );
        }
    }
}
