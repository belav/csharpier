using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CatchClause
    {
        public static Doc Print(CatchClauseSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.CatchKeyword),
                Doc.Group(
                    node.Declaration != null
                        ? Doc.Concat(
                              " ",
                              Token.Print(node.Declaration.OpenParenToken),
                              Node.Print(node.Declaration.Type),
                              node.Declaration.Identifier.Kind() != SyntaxKind.None
                                  ? " "
                                  : Doc.Null,
                              Token.Print(node.Declaration.Identifier),
                              Token.Print(node.Declaration.CloseParenToken)
                          )
                        : Doc.Null,
                    node.Filter != null
                        ? Doc.Indent(
                              Doc.Line,
                              Token.PrintWithSuffix(node.Filter.WhenKeyword, " "),
                              Token.Print(node.Filter.OpenParenToken),
                              Doc.Group(
                                  Doc.Indent(Node.Print(node.Filter.FilterExpression)),
                                  Doc.SoftLine
                              ),
                              Token.Print(node.Filter.CloseParenToken)
                          )
                        : Doc.Null
                ),
                Block.Print(node.Block)
            );
        }
    }
}
