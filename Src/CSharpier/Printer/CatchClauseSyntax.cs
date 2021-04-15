using System.Collections.Generic;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCatchClauseSyntax(CatchClauseSyntax node)
        {
            var docs = new List<Doc>();
            docs.Add(SyntaxTokens.Print(node.CatchKeyword));
            if (node.Declaration != null)
            {
                docs.Add(
                    " ",
                    SyntaxTokens.Print(node.Declaration.OpenParenToken),
                    this.Print(node.Declaration.Type),
                    node.Declaration.Identifier.RawKind != 0 ? " " : Doc.Null,
                    SyntaxTokens.Print(node.Declaration.Identifier),
                    SyntaxTokens.Print(node.Declaration.CloseParenToken)
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
                    SyntaxTokens.Print(node.Filter.OpenParenToken),
                    this.Print(node.Filter.FilterExpression),
                    SyntaxTokens.Print(node.Filter.CloseParenToken)
                );
            }

            docs.Add(this.PrintBlockSyntax(node.Block));
            return Docs.Concat(docs);
        }
    }
}
