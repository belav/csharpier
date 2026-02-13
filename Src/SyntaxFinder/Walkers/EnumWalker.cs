using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxFinder.Walkers;

public class EnumWalker(string file) : SyntaxFinderWalker(file)
{
    public static readonly ConcurrentDictionary<string, List<string>> MembersInType = new();
    public static int Total;
    public static int Matching;

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        this.VisitType(node);
        base.VisitEnumMemberDeclaration(node);
    }

    private void VisitType(EnumMemberDeclarationSyntax node)
    {
        if (!node.AttributeLists.Any() || node.AttributeLists.Span.Length > 8)
        {
            return;
        }

        Interlocked.Increment(ref Total);
        if (
            node.SyntaxTree.GetLineSpan(node.AttributeLists.First().Span).StartLinePosition.Line
            == node.SyntaxTree.GetLineSpan(node.Identifier.Span).StartLinePosition.Line
        )
        {
            Interlocked.Increment(ref Matching);
            this.WriteCode(node);
        }
    }

    private static bool IsMultiline(SyntaxNode syntaxNode)
    {
        var lineSpan = syntaxNode.SyntaxTree.GetLineSpan(syntaxNode.Span);
        return lineSpan.StartLinePosition.Line != lineSpan.EndLinePosition.Line;
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
