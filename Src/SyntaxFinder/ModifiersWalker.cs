namespace SyntaxFinder;

using System.Collections.Concurrent;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ModifiersWalker : SyntaxFinderWalker
{
    public ModifiersWalker(string file)
        : base(file) { }

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
