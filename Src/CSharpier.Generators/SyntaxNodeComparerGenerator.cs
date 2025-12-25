using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Generators;

[Generator]
public class SyntaxNodeComparerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider.Select(
            (compilation, _) => ValidNodeTypes.Get(compilation)
        );

        context.RegisterSourceOutput(
            compilationProvider,
            static (context, entityClasses) => Execute(context, entityClasses)
        );
    }

    // this would probably be easier to understand as a scriban template but is a lot of effort
    // to switch and doesn't really change at this point
    public static void Execute(SourceProductionContext context, IEnumerable<INamedTypeSymbol> types)
    {
        context.AddSource("SyntaxNodeComparer.generated", GenerateSource(types.ToList()));
    }

    private static string GenerateSource(List<INamedTypeSymbol> types)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(
            """
#pragma warning disable
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp
{
    internal partial class SyntaxNodeComparer
    {
        private CompareResult Compare(
            (SyntaxNode originalNode, SyntaxNode originalParent) original,
            (SyntaxNode formattedNode, SyntaxNode formattedParent) formatted
        )
        {
            var (originalNode, originalParent) = original;
            var (formattedNode, formattedParent) = formatted;
            if (originalNode == null && formattedNode == null)
            {
                return Equal;
            }

            if (originalNode == null || formattedNode == null)
            {
                return NotEqual(originalParent, formattedParent);
            }

            var type = originalNode?.GetType();
            if (type != formattedNode?.GetType())
            {
                return NotEqual(originalNode, formattedNode);
            }

            if (originalNode.RawKind != formattedNode.RawKind)
            {
                return NotEqual(originalNode, formattedNode);
            }

            switch (originalNode)
            {
"""
        );

        foreach (var syntaxNodeType in types)
        {
            var lowerCaseName =
                syntaxNodeType.Name[0].ToString().ToLower(CultureInfo.InvariantCulture)
                + syntaxNodeType.Name[1..];

            if (syntaxNodeType.Name == nameof(UsingDirectiveSyntax))
            {
                sourceBuilder.AppendLine(
                    $"""
                case {syntaxNodeType.Name} {lowerCaseName}:
                    if (this.ReorderedUsingsWithDisabledText)
                        return Equal;
                    return this.Compare{syntaxNodeType.Name}({lowerCaseName}, formattedNode as {syntaxNodeType.Name});
"""
                );
            }
            else
            {
                sourceBuilder.AppendLine(
                    $"""
             case {syntaxNodeType.Name} {lowerCaseName}:
                 return this.Compare{syntaxNodeType.Name}({lowerCaseName}, formattedNode as {syntaxNodeType.Name});
"""
                );
            }
        }

        sourceBuilder.AppendLine(
            """
                default:
#if DEBUG
                    throw new Exception("Can't handle " + originalNode.GetType().Name + ". May need to rerun CSharpier.FakeGenerators");
#else
                    return Equal;
#endif
            }
        }
        
"""
        );

        foreach (var syntaxNodeType in types)
        {
            GenerateMethod(sourceBuilder, syntaxNodeType);
        }

        sourceBuilder.AppendLine(
            @"    }
}"
        );

        return sourceBuilder.ToString();
    }

    private static void GenerateMethod(StringBuilder sourceBuilder, INamedTypeSymbol type)
    {
        sourceBuilder.AppendLine(
            $$"""
                  private CompareResult Compare{{type.Name}}({{type.Name}} originalNode, {{type.Name}} formattedNode)
                  {
                       CompareResult result;
            """
        );

        if (
            type.BaseType?.ToDisplayString()
                == "Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax"
            || type.ToDisplayString()
                == "Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax"
        )
        {
            sourceBuilder.AppendLine(
                $"            if (CompareFullSpan(originalNode, formattedNode)) return Equal;"
            );
        }

        foreach (var propertySymbol in type.GetAllProperties().OrderBy(o => o.Name))
        {
            var propertyName = propertySymbol.Name;

            if (
                propertyName
                is "Language"
                    or "Parent"
                    or "HasLeadingTrivia" // we modify/remove whitespace and new lines so we can't look at these properties.
                    or "HasTrailingTrivia"
                    or "ParentTrivia"
                    or "Arity"
                    or "SpanStart"
                    or "KindText"
            )
            {
                continue;
            }

            var camelCaseName = CamelCaseName(propertyName);
            if (propertySymbol.Type is not INamedTypeSymbol propertyType)
            {
                continue;
            }

            if (
                Ignored.Properties.Contains(camelCaseName)
                || Ignored.Types.Contains(propertyType.Name)
                || (
                    Ignored.PropertiesByType.ContainsKey(type.Name)
                    && Ignored.PropertiesByType[type.Name].Contains(camelCaseName)
                )
            )
            {
                continue;
            }

            if (propertyType.SpecialType is SpecialType.System_Boolean or SpecialType.System_Int32)
            {
                sourceBuilder.AppendLine(
                    $"            if (originalNode.{propertyName} != formattedNode.{propertyName}) return NotEqual(originalNode, formattedNode);"
                );
            }
            else if (propertyType.Name == nameof(SyntaxToken))
            {
                sourceBuilder.AppendLine(
                    $"            result = this.Compare(originalNode.{propertyName}, formattedNode.{propertyName}, originalNode, formattedNode);"
                );
                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");
            }
            else if (propertyType.Name == nameof(SyntaxTrivia))
            {
                sourceBuilder.AppendLine(
                    $"            result = this.Compare(originalNode.{propertyName}, formattedNode.{propertyName});"
                );
                sourceBuilder.AppendLine("            if (result.IsInvalid) return result;");
            }
            else if (propertyType.InheritsFrom(nameof(CSharpSyntaxNode)))
            {
                sourceBuilder.AppendLine(
                    $"            originalStack.Push((originalNode.{propertyName}, originalNode));"
                );
                sourceBuilder.AppendLine(
                    $"            formattedStack.Push((formattedNode.{propertyName}, formattedNode));"
                );
            }
            else if (
                propertyType.Name == nameof(SyntaxTokenList)
                || (
                    propertyType.IsGenericType
                    && propertyType
                        .ConstructedFrom.ToDisplayString()
                        .StartsWith("Microsoft.CodeAnalysis.SyntaxList<")
                )
            )
            {
                if (
                    propertyType.IsGenericType
                    && propertyType.TypeArguments[0].Name == nameof(UsingDirectiveSyntax)
                )
                {
                    sourceBuilder.AppendLine(
                        $"            result = this.CompareUsingDirectives(originalNode.{propertyName}, formattedNode.{propertyName}, originalNode, formattedNode);"
                    );
                }
                else
                {
                    var compare =
                        propertyType.Name == nameof(SyntaxTokenList)
                            ? "CompareFunc"
                            : "static (_, _) => default";
                    if (propertyName == "Modifiers")
                    {
                        propertyName += ".OrderBy(o => o.Text).ToArray()";
                    }

                    sourceBuilder.AppendLine(
                        $"            result = this.CompareLists(originalNode.{propertyName}, formattedNode.{propertyName}, {compare}, o => o.Span, originalNode.Span, formattedNode.Span);"
                    );
                }

                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");
            }
            else if (
                propertyType.IsGenericType
                && propertyType
                    .ConstructedFrom.ToDisplayString()
                    .StartsWith("Microsoft.CodeAnalysis.SeparatedSyntaxList<")
            )
            {
                sourceBuilder.AppendLine(
                    $"            result = this.CompareLists(originalNode.{propertyName}, formattedNode.{propertyName}, static (_, _) => default, o => o.Span, originalNode.Span, formattedNode.Span);"
                );
                sourceBuilder.AppendLine("            if (result.IsInvalid) return result;");

                // Omit the last separator when comparing the original node with the formatted node, as it legitimately may be added or removed
                sourceBuilder.AppendLine(
                    $"            result = this.CompareLists(AllSeparatorsButLast(originalNode.{propertyName}), AllSeparatorsButLast(formattedNode.{propertyName}), CompareFunc, o => o.Span, originalNode.Span, formattedNode.Span);"
                );
                sourceBuilder.AppendLine("            if (result.IsInvalid) return result;");
            }
            else
            {
                sourceBuilder.AppendLine("// " + propertyName);
                sourceBuilder.AppendLine("// " + propertyType.ConstructedFrom.ToDisplayString());
                sourceBuilder.AppendLine("// " + propertyType.ConstructedFrom);
                sourceBuilder.AppendLine("// " + propertyType.Name);
            }
        }

        sourceBuilder.AppendLine("            return Equal;");
        sourceBuilder.AppendLine("        }");
    }

    private static string CamelCaseName(string name)
    {
        return name.ToLower(CultureInfo.InvariantCulture)[0] + name[1..];
    }
}
