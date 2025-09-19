using Microsoft.CodeAnalysis;

namespace CSharpier.Generators;

public static class INamedTypeSymbolExtensions
{
    public static bool InheritsFrom(this INamedTypeSymbol classSymbol, string typeName)
    {
        var baseType = classSymbol;
        while (baseType != null)
        {
            if (baseType.Name == typeName)
            {
                return true;
            }

            baseType = baseType.BaseType;
        }
        return false;
    }

    public static IEnumerable<IPropertySymbol> GetAllProperties(this INamedTypeSymbol type)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);

        var current = type;
        while (current != null)
        {
            foreach (var prop in current.GetMembers().OfType<IPropertySymbol>())
            {
                // Skip if already seen by a more derived type
                if (seen.Add(prop.Name))
                {
                    yield return prop;
                }
            }

            current = current.BaseType;
        }
    }
}
