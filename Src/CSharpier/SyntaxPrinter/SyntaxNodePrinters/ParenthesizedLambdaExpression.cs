using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class ParenthesizedLambdaExpression
    {
        public static Doc Print(ParenthesizedLambdaExpressionSyntax node)
        {
            var docs = new List<Doc>
            {
                AttributeLists.Print(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                node.ReturnType != null ? Doc.Concat(Node.Print(node.ReturnType), " ") : Doc.Null,
                ParameterList.Print(node.ParameterList),
                " ",
                Token.Print(node.ArrowToken)
            };
            if (node.ExpressionBody != null)
            {
                docs.Add(Doc.Group(Doc.Indent(Doc.Line, Node.Print(node.ExpressionBody))));
            }
            else if (node.Block != null)
            {
                if (node.Block.Statements.Count > 0)
                {
                    docs.Add(Doc.HardLine);
                }
                else
                {
                    docs.Add(" ");
                }

                docs.Add(Block.Print(node.Block));
            }

            return Doc.Concat(docs);
        }
    }
}
