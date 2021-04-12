using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCatchClauseSyntax(CatchClauseSyntax node)
        {
            var docs = new List<Doc>();
            docs.Add(this.PrintSyntaxToken(node.CatchKeyword));
            if (node.Declaration != null)
            {
                docs.Add(
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
                docs.Add(
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

            docs.Add(this.PrintBlockSyntax(node.Block));
            return Docs.Concat(docs);
        }
    }
}
