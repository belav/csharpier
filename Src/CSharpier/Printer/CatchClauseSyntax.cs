using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCatchClauseSyntax(CatchClauseSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintSyntaxToken(node.CatchKeyword));
            if (node.Declaration != null)
            {
                parts.Push(
                    " ",
                    this.PrintSyntaxToken(node.Declaration.OpenParenToken),
                    this.Print(node.Declaration.Type),
                    node.Declaration.Identifier.RawKind != 0 ? " " : Doc.Null,
                    this.PrintSyntaxToken(node.Declaration.Identifier),
                    this.PrintSyntaxToken(node.Declaration.CloseParenToken)
                );
            }

            if (node.Filter != null)
            {
                parts.Push(
                    " ",
                    this.PrintSyntaxToken(
                        node.Filter.WhenKeyword,
                        afterTokenIfNoTrailing: " "
                    ),
                    this.PrintSyntaxToken(node.Filter.OpenParenToken),
                    this.Print(node.Filter.FilterExpression),
                    this.PrintSyntaxToken(node.Filter.CloseParenToken)
                );
            }

            parts.Push(this.PrintBlockSyntax(node.Block));
            return Concat(parts);
        }
    }
}
