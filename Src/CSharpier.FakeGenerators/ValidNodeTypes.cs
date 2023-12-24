using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.FakeGenerators;

public static class ValidNodeTypes
{
    public static IList<Type> Get()
    {
        return typeof(CompilationUnitSyntax)
            .Assembly
            .GetTypes()
            .Where(
                o =>
                    !o.IsAbstract
                    && typeof(CSharpSyntaxNode).IsAssignableFrom(o)
                    && !Ignored.UnsupportedNodes.Contains(o.Name)
            )
            .OrderBy(o => o.Name)
            .ToList();
    }
}
