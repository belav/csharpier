using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.FakeGenerators;

using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SyntaxNodeComparerGenerator
{
    // this would probably be easier to understand as a scriban template but is a lot of effort
    // to switch and doesn't really change at this point
    public void Execute(CodeContext context)
    {
        context.AddSource("SyntaxNodeComparer.generated", GenerateSource());
    }

    private static string GenerateSource()
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(
            """
#pragma warning disable CS0168
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
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

        var syntaxNodeTypes = ValidNodeTypes.Get();

        foreach (var syntaxNodeType in syntaxNodeTypes)
        {
            var lowerCaseName =
                syntaxNodeType.Name[0].ToString().ToLower() + syntaxNodeType.Name[1..];

            if (syntaxNodeType == typeof(UsingDirectiveSyntax))
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
                    throw new Exception("Can't handle " + originalNode.GetType().Name);
#else
                    return Equal;
#endif
            }
        }
        
"""
        );

        foreach (var syntaxNodeType in syntaxNodeTypes)
        {
            GenerateMethod(sourceBuilder, syntaxNodeType);
        }

        sourceBuilder.AppendLine(
            @"    }
}"
        );

        return sourceBuilder.ToString();
    }

    private static void GenerateMethod(StringBuilder sourceBuilder, Type type)
    {
        sourceBuilder.AppendLine(
            $$"""
      private CompareResult Compare{{type.Name}}({{type.Name}} originalNode, {{type.Name}} formattedNode)
      {
          CompareResult result;
"""
        );

        foreach (var propertyInfo in type.GetProperties().OrderBy(o => o.Name))
        {
            var propertyName = propertyInfo.Name;

            if (
                propertyName
                is "Language"
                    or "Parent"
                    or "HasLeadingTrivia" // we modify/remove whitespace and new lines so we can't look at these properties.
                    or "HasTrailingTrivia"
                    or "ParentTrivia"
                    or "Arity"
                    or "SpanStart"
            )
            {
                continue;
            }

            var camelCaseName = CamelCaseName(propertyName);
            var propertyType = propertyInfo.PropertyType;

            if (
                Ignored.Properties.Contains(camelCaseName)
                || Ignored.Types.Contains(propertyType)
                || (
                    Ignored.PropertiesByType.ContainsKey(type)
                    && Ignored.PropertiesByType[type].Contains(camelCaseName)
                )
            )
            {
                continue;
            }

            if (propertyType == typeof(bool) || propertyType == typeof(Int32))
            {
                sourceBuilder.AppendLine(
                    $@"            if (originalNode.{propertyName} != formattedNode.{propertyName}) return NotEqual(originalNode, formattedNode);"
                );
            }
            else if (propertyType == typeof(SyntaxToken))
            {
                sourceBuilder.AppendLine(
                    $"            result = this.Compare(originalNode.{propertyName}, formattedNode.{propertyName}, originalNode, formattedNode);"
                );
                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");
            }
            else if (propertyType == typeof(SyntaxTrivia))
            {
                sourceBuilder.AppendLine(
                    $"            result = this.Compare(originalNode.{propertyName}, formattedNode.{propertyName});"
                );
                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");
            }
            else if (typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType))
            {
                sourceBuilder.AppendLine(
                    $"            originalStack.Push((originalNode.{propertyName}, originalNode));"
                );
                sourceBuilder.AppendLine(
                    $"            formattedStack.Push((formattedNode.{propertyName}, formattedNode));"
                );
            }
            else if (
                propertyType == typeof(SyntaxTokenList)
                || (
                    propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>)
                )
            )
            {
                if (
                    propertyType.IsGenericType
                    && propertyType.GenericTypeArguments[0] == typeof(UsingDirectiveSyntax)
                )
                {
                    sourceBuilder.AppendLine(
                        $"            result = this.CompareUsingDirectives(originalNode.{propertyName}, formattedNode.{propertyName}, originalNode, formattedNode);"
                    );
                }
                else
                {
                    var compare = propertyType == typeof(SyntaxTokenList) ? "Compare" : "null";
                    if (propertyName == "Modifiers")
                    {
                        propertyName += ".OrderBy(o => o.Text).ToList()";
                    }
                    sourceBuilder.AppendLine(
                        $"            result = this.CompareLists(originalNode.{propertyName}, formattedNode.{propertyName}, {compare}, o => o.Span, originalNode.Span, formattedNode.Span);"
                    );
                }

                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");
            }
            else if (
                propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>)
            )
            {
                sourceBuilder.AppendLine(
                    $"            result = this.CompareLists(originalNode.{propertyName}, formattedNode.{propertyName}, null, o => o.Span, originalNode.Span, formattedNode.Span);"
                );
                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");

                // Omit the last separator when comparing the original node with the formatted node, as it legitimately may be added or removed
                sourceBuilder.AppendLine(
                    $"            result = this.CompareLists(originalNode.{propertyName}.GetSeparators().Take(originalNode.{propertyName}.Count() - 1).ToList(), formattedNode.{propertyName}.GetSeparators().Take(formattedNode.{propertyName}.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);"
                );
                sourceBuilder.AppendLine($"            if (result.IsInvalid) return result;");
            }
        }
        sourceBuilder.AppendLine("            return Equal;");
        sourceBuilder.AppendLine("        }");
    }

    private static string CamelCaseName(string name)
    {
        return name.ToLower()[0] + name[1..];
    }
}
