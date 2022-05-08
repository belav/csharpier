using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Generators;

public static class ValidNodeTypes
{
    public static IEnumerable<Type> Get()
    {
        return typeof(CompilationUnitSyntax).Assembly
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
