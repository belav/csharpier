using System.Globalization;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxFinder.Walkers;

public class ObjectInitializerWalker(string file) : CSharpSyntaxWalker
{
    private static int total;
    private static int matching;
    private static double totalExpressions;
    private static HashSet<string> matchingFiles = new();

    public override void VisitInitializerExpression(InitializerExpressionSyntax node)
    {
        if (node.Kind() is SyntaxKind.ObjectInitializerExpression)
        {
            this.VisitNode(node);
        }

        base.VisitInitializerExpression(node);
    }

    private void VisitNode(InitializerExpressionSyntax node)
    {
        Interlocked.Increment(ref total);

        totalExpressions += node.Expressions.Count;

        if (
            node.Expressions.Any(o =>
                o.GetLeadingTrivia().Any(o => o.Kind() is SyntaxKind.EndOfLineTrivia)
            )
        )
        {
            Interlocked.Increment(ref matching);
            matchingFiles.Add(
                node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line + " - " + file
            );
        }
    }

    public static void WriteResult()
    {
        foreach (var file in matchingFiles)
        {
            Console.WriteLine(file);
        }
        ResultWriter.WriteResult(
            "Avg Expressions",
            (totalExpressions / total).ToString(CultureInfo.InvariantCulture)
        );

        ResultWriter.WriteMatching(total, matching);
    }
}
