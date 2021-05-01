using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ParenthesizedLambdaExpression
    {
        public static Doc Print(ParenthesizedLambdaExpressionSyntax node)
        {
            var docs = new List<Doc>
            {
                Modifiers.Print(node.Modifiers),
                ParameterList.Print(node.ParameterList),
                " ",
                Token.PrintWithSuffix(
                    node.ArrowToken,
                    " "
                )
            };
            if (node.ExpressionBody != null)
            {
                docs.Add(Node.Print(node.ExpressionBody));
            }
            else if (node.Block != null)
            {
                docs.Add(Block.Print(node.Block));
            }

            return Doc.Concat(docs);
        }
    }
}
