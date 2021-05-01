using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCatchClauseSyntax(CatchClauseSyntax node)
        {
            var docs = new List<Doc>();
            docs.Add(Token.Print(node.CatchKeyword));
            if (node.Declaration != null)
            {
                docs.Add(
                    " ",
                    Token.Print(node.Declaration.OpenParenToken),
                    this.Print(node.Declaration.Type),
                    node.Declaration.Identifier.RawKind != 0 ? " " : Doc.Null,
                    Token.Print(node.Declaration.Identifier),
                    Token.Print(node.Declaration.CloseParenToken)
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
                    Token.Print(node.Filter.OpenParenToken),
                    this.Print(node.Filter.FilterExpression),
                    Token.Print(node.Filter.CloseParenToken)
                );
            }

            docs.Add(this.PrintBlockSyntax(node.Block));
            return Doc.Concat(docs);
        }
    }
}
