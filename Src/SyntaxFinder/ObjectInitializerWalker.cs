namespace SyntaxFinder;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ObjectInitializerWalker : CSharpSyntaxWalker
{
    private static int total;
    private static int matching;
    private static double totalExpressions;
    private readonly string file;
    private static HashSet<string> matchingFiles = new();

    public ObjectInitializerWalker(string file)
    {
        this.file = file;
    }

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
        total++;

        totalExpressions += node.Expressions.Count;

        if (
            node.Expressions.Any(
                o => o.GetLeadingTrivia().Any(o => o.Kind() is SyntaxKind.EndOfLineTrivia)
            )
        )
        {
            matching++;
            matchingFiles.Add(
                node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line + " - " + this.file
            );
        }
    }

    public static void WriteResult()
    {
        foreach (var file in matchingFiles)
        {
            Console.WriteLine(file);
        }
        ResultWriter.WriteResult("Avg Expressions", (totalExpressions / total).ToString());

        ResultWriter.WriteMatching(total, matching);
    }
}
