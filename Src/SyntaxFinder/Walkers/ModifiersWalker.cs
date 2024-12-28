using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxFinder.Walkers;

public class ModifiersWalker(string file) : SyntaxFinderWalker(file)
{
    public override void VisitParenthesizedLambdaExpression(
        ParenthesizedLambdaExpressionSyntax node
    )
    {
        if (node.Modifiers.Count > 1)
        {
            this.WriteFilePath(true);
            this.WriteCode(node);
        }
        base.VisitParenthesizedLambdaExpression(node);
    }
}
