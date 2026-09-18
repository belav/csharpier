using AwesomeAssertions;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Tests.CSharp.SyntaxPrinter;

public class ArgumentListLikeTests
{
    [Test]
    public void Single_Lambda_Argument_Should_Print_Its_Body_Once()
    {
        var doc = PrintDoc("One(a => a.Value);");

        var ifBreak = AllDocs(doc)
            .OfType<IfBreak>()
            .Single(o =>
                o.GroupId is not null
                && o.GroupId.StartsWith("LambdaArguments", StringComparison.Ordinal)
            );

        var indented = ifBreak.BreakContents.Should().BeOfType<IndentDoc>().Subject;
        var grouped = indented.Contents.Should().BeOfType<Group>().Subject;

        grouped.Contents.Should().BeSameAs(ifBreak.FlatContents);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public void Nested_Lambda_Arguments_Should_Print_Their_Bodies_Once_Each(int depth)
    {
        var doc = PrintDoc(NestedLambdas(depth));

        var groupIds = AllDocs(doc)
            .Select(o =>
                o switch
                {
                    Group group => group.GroupId,
                    IfBreak ifBreak => ifBreak.GroupId,
                    _ => null,
                }
            )
            .Where(o => o is not null && o.StartsWith("LambdaArguments", StringComparison.Ordinal))
            .Distinct()
            .ToList();

        groupIds.Should().HaveCount(depth);
    }

    private static string NestedLambdas(int depth)
    {
        var result = "a";
        for (var x = 0; x < depth; x++)
        {
            result = $"Call(a => {result})";
        }

        return result + ";";
    }

    private static Doc PrintDoc(string statement)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            $$"""
            class ClassName
            {
                void MethodName()
                {
                    {{statement}}
                }
            }
            """
        );

        return Node.Print(syntaxTree.GetRoot(), new CSharpPrintingContext { LineEnding = "\n" });
    }

    private static IEnumerable<Doc> AllDocs(Doc doc)
    {
        var stack = new Stack<Doc>();
        stack.Push(doc);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            switch (current)
            {
                case ConditionalGroup conditionalGroup:
                    foreach (var option in conditionalGroup.Options)
                    {
                        stack.Push(option);
                    }

                    break;
                case IHasContents hasContents:
                    stack.Push(hasContents.Contents);
                    break;
                case Concat concat:
                    foreach (var child in concat.Contents)
                    {
                        stack.Push(child);
                    }

                    break;
                case IfBreak ifBreak:
                    stack.Push(ifBreak.BreakContents);
                    stack.Push(ifBreak.FlatContents);
                    break;
                case AlwaysFits alwaysFits:
                    stack.Push(alwaysFits.Contents);
                    break;
            }
        }
    }
}
