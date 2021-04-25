using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedLambdaExpressionSyntax(
            ParenthesizedLambdaExpressionSyntax node
        ) {
            var docs = new List<Doc>
            {
                Modifiers.Print(node.Modifiers),
                this.PrintParameterListSyntax(node.ParameterList),
                " ",
                this.PrintSyntaxToken(
                    node.ArrowToken,
                    afterTokenIfNoTrailing: " "
                )
            };
            if (node.ExpressionBody != null)
            {
                docs.Add(this.Print(node.ExpressionBody));
            }
            else if (node.Block != null)
            {
                docs.Add(this.PrintBlockSyntax(node.Block));
            }

            return Docs.Concat(docs);
        }
    }
}
