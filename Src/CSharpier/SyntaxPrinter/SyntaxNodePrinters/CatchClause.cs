using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CatchClause
    {
        public static Doc Print(CatchClauseSyntax node)
        {
            var docs = new List<Doc>();
            docs.Add(Token.Print(node.CatchKeyword));
            if (node.Declaration != null)
            {
                docs.Add(
                    " ",
                    Token.Print(node.Declaration.OpenParenToken),
                    Node.Print(node.Declaration.Type),
                    node.Declaration.Identifier.RawKind != 0 ? " " : Doc.Null,
                    Token.Print(node.Declaration.Identifier),
                    Token.Print(node.Declaration.CloseParenToken)
                );
            }

            if (node.Filter != null)
            {
                docs.Add(
                    " ",
                    Token.Print(node.Filter.WhenKeyword, " "),
                    Token.Print(node.Filter.OpenParenToken),
                    Node.Print(node.Filter.FilterExpression),
                    Token.Print(node.Filter.CloseParenToken)
                );
            }

            docs.Add(Block.Print(node.Block));
            return Doc.Concat(docs);
        }
    }
}
