using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxFinder.Walkers;

public class NestedConditionalExpressionsWalker(string file) : SyntaxFinderWalker(file)
{
    public static readonly ConcurrentDictionary<string, List<string>> MembersInType = new();
    public static int Total;
    public static int Matching;

    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        if (node.WhenTrue is ConditionalExpressionSyntax)
        {
            this.WriteFilePath();
            // this.WriteCode(node.Parent!);
            Interlocked.Increment(ref Total);
        }
    }

    public static void WriteResult()
    {
        foreach (var entry in MembersInType)
        {
            Console.WriteLine(entry.Key);
            foreach (var member in entry.Value.OrderBy(o => o))
            {
                Console.WriteLine("    " + member);
            }
        }

        ResultWriter.WriteMatching(Total, Matching);
    }
}
