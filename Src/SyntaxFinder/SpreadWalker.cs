namespace SyntaxFinder;

using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SpreadWalker : CSharpSyntaxWalker
{
    private static int total;
    private static int matching;
    private readonly string file;

    public SpreadWalker(string file)
    {
        this.file = file;
    }

    public override void VisitSpreadElement(SpreadElementSyntax node)
    {
        if (node.Kind() is SyntaxKind.SpreadElement)
        {
            this.VisitNode(node);
        }

        base.VisitSpreadElement(node);
    }

    private void VisitNode(SpreadElementSyntax node)
    {
        Console.WriteLine(node.Parent!.SyntaxTree.GetText().ToString(node.Parent!.FullSpan));
        Interlocked.Increment(ref total);

        if (node.Expression.GetLeadingTrivia().Any(o => o.Kind() is SyntaxKind.WhitespaceTrivia))
        {
            Interlocked.Increment(ref matching);
        }
    }

    public static void WriteResult()
    {
        ResultWriter.WriteMatching(total, matching);
    }
}
