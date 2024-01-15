namespace SyntaxFinder;

using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ExampleWalker : CSharpSyntaxWalker
{
    public static readonly ConcurrentDictionary<string, List<string>> MembersInType = new();
    public static int Total;
    public static int Matching;
    private readonly string file;
    private bool wroteFile;
    private readonly int maxCodeWrites = 250;
    private int codeWrites = 0;

    public ExampleWalker(string file)
    {
        this.file = file;
    }

    public override void VisitCompilationUnit(CompilationUnitSyntax node)
    {
        this.VisitType(node, node.Members);
        base.VisitCompilationUnit(node);
    }

    private void VisitType(CSharpSyntaxNode node, SyntaxList<MemberDeclarationSyntax> members)
    {
        foreach (var member in members)
        {
            MembersInType.AddOrUpdate(
                node.GetType().Name,
                new List<string>(),
                (key, list) =>
                {
                    if (!list.Contains(member.GetType().Name))
                    {
                        list.Add(member.GetType().Name);
                    }

                    return list;
                }
            );
        }
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (
            (
                node.Parent is TypeDeclarationSyntax typeDeclarationSyntax
                && node == typeDeclarationSyntax.Members.First()
            ) || node.GetLeadingTrivia().Any(o => o.IsComment() || node.AttributeLists.Any())
        )
        {
            base.VisitMethodDeclaration(node);
            return;
        }

        Interlocked.Increment(ref Total);
        this.WriteCode(node.Parent!);

        if (node.GetLeadingTrivia().Any(o => o.Kind() is SyntaxKind.EndOfLineTrivia))
        {
            Interlocked.Increment(ref Matching);
        }

        base.VisitMethodDeclaration(node);
    }

    private void WriteCode(SyntaxNode syntaxNode)
    {
        if (this.codeWrites < this.maxCodeWrites)
        {
            Interlocked.Increment(ref this.codeWrites);
            Console.WriteLine(syntaxNode.SyntaxTree.GetText().ToString(syntaxNode.Span));
        }
    }

    private void WriteFilePath()
    {
        if (!this.wroteFile)
        {
            Console.WriteLine(this.file);
            this.wroteFile = true;
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
