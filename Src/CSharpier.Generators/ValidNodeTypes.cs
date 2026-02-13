using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Generators;

public static class ValidNodeTypes
{
    public static IEnumerable<INamedTypeSymbol> Get(Compilation compilation)
    {
        foreach (var reference in compilation.References)
        {
            var assembly = compilation.GetAssemblyOrModuleSymbol(reference) as IAssemblySymbol;
            if (assembly?.Name != "Microsoft.CodeAnalysis.CSharp")
            {
                continue;
            }

            var childNamespace = GetNamespaceByName(
                assembly,
                "Microsoft.CodeAnalysis.CSharp.Syntax"
            );
            foreach (var type in childNamespace.GetTypeMembers().OrderBy(o => o.Name))
            {
                if (
                    type.DeclaredAccessibility == Accessibility.Public
                    && !type.IsAbstract
                    && type.InheritsFrom(nameof(CSharpSyntaxNode))
                    && !Ignored.UnsupportedNodes.Contains(type.Name)
                )
                {
                    yield return type;
                }
            }
        }
    }

    private static INamespaceSymbol GetNamespaceByName(
        this IAssemblySymbol assembly,
        string namespaceName
    )
    {
        var parts = namespaceName.Split('.');
        var current = assembly.GlobalNamespace;

        foreach (var part in parts)
        {
            current = current.GetNamespaceMembers().FirstOrDefault(o => o.Name == part);
            if (current == null)
            {
                throw new Exception("Cannot find " + namespaceName);
            }
        }

        return current;
    }
}
