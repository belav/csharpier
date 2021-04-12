using System.Collections.Generic;
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
                this.PrintModifiers(node.Modifiers),
                this.PrintParameterListSyntax(node.ParameterList),
                Docs.SpaceIfNoPreviousComment,
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
