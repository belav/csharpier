using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Generators;

[Generator]
public class SyntaxNodeJsonWriterGenerator : IIncrementalGenerator
{
    private readonly List<string> missingTypes = [];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider.Select(
            (compilation, _) => ValidNodeTypes.Get(compilation)
        );

        context.RegisterSourceOutput(compilationProvider, this.Execute);
    }

    // this would probably be easier to understand as a scriban template but is a lot of effort
    // to switch and doesn't really change at this point
    public void Execute(SourceProductionContext context, IEnumerable<INamedTypeSymbol> types)
    {
        context.AddSource("SyntaxNodeJsonWriter.generated", this.GenerateSource(types.ToList()));
    }

    private string GenerateSource(List<INamedTypeSymbol> types)
    {
        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine("#pragma warning disable");
        sourceBuilder.AppendLine("using System.Collections.Generic;");
        sourceBuilder.AppendLine("using System.IO;");
        sourceBuilder.AppendLine("using System.Linq;");
        sourceBuilder.AppendLine("using System.Text;");
        sourceBuilder.AppendLine("using Microsoft.CodeAnalysis;");
        sourceBuilder.AppendLine("using Microsoft.CodeAnalysis.CSharp;");
        sourceBuilder.AppendLine("using Microsoft.CodeAnalysis.CSharp.Syntax;");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine("namespace CSharpier.Core.CSharp");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine("    internal static partial class SyntaxNodeJsonWriter");
        sourceBuilder.AppendLine("    {");

        sourceBuilder.AppendLine(
            $"        public static void WriteSyntaxNode(StringBuilder builder, SyntaxNode syntaxNode)"
        );
        sourceBuilder.AppendLine("        {");
        foreach (var syntaxNodeType in types)
        {
            sourceBuilder.AppendLine(
                $"            if (syntaxNode is {syntaxNodeType.Name}) Write{syntaxNodeType.Name}(builder, syntaxNode as {syntaxNodeType.Name});"
            );
        }

        sourceBuilder.AppendLine("        }");
        sourceBuilder.AppendLine();

        foreach (var syntaxNodeType in types)
        {
            this.GenerateMethod(sourceBuilder, syntaxNodeType);
        }

        // TODO
        // this.GenerateMethod(sourceBuilder, typeof(SyntaxToken));
        // this.GenerateMethod(sourceBuilder, typeof(SyntaxTrivia));

        sourceBuilder.AppendLine("    }");
        sourceBuilder.AppendLine("}");

        if (this.missingTypes.Count != 0)
        {
            throw new Exception(
#pragma warning disable RS1035
                Environment.NewLine + string.Join(Environment.NewLine, this.missingTypes)
#pragma warning restore RS1035
            );
        }

        return sourceBuilder.ToString();
    }

    private void GenerateMethod(StringBuilder sourceBuilder, INamedTypeSymbol type)
    {
        sourceBuilder.AppendLine(
            $$"""
                    public static void Write{{type.Name}}(StringBuilder builder, {{type.Name}} syntaxNode)
                    {
                        builder.Append("{");
                        var properties = new List<string>();
                        properties.Add($"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\"");
                        properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            """
        );

        if (type.Name == nameof(SyntaxTrivia))
        {
            sourceBuilder.AppendLine(
                "            properties.Add(WriteString(\"text\", syntaxNode.ToString()));"
            );
        }

        foreach (var propertySymbol in type.GetAllProperties().OrderBy(o => o.Name))
        {
            var propertyName = propertySymbol.Name;
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

            if (propertyType.SpecialType is SpecialType.System_Boolean)
            {
                sourceBuilder.AppendLine(
                    $"            properties.Add(WriteBoolean(\"{camelCaseName}\", syntaxNode.{propertyName}));"
                );
            }
            else if (propertyType.SpecialType is SpecialType.System_String)
            {
                sourceBuilder.AppendLine(
                    $"            properties.Add(WriteString(\"{camelCaseName}\", syntaxNode.{propertyName}));"
                );
            }
            else if (propertyType.SpecialType is SpecialType.System_Int32)
            {
                sourceBuilder.AppendLine(
                    $"            properties.Add(WriteInt(\"{camelCaseName}\", syntaxNode.{propertyName}));"
                );
            }
            else if (
                propertyType.InheritsFrom(nameof(CSharpSyntaxNode))
                || propertyType.Name == nameof(SyntaxToken)
                || propertyType.Name == nameof(SyntaxTrivia)
            )
            {
                var methodName = "WriteSyntaxNode";
                if (propertyType.Name == nameof(SyntaxToken))
                {
                    methodName = "WriteSyntaxToken";
                }
                else if (propertyType.Name == nameof(SyntaxTrivia))
                {
                    methodName = "WriteSyntaxTrivia";
                }
                else if (!propertyType.IsAbstract)
                {
                    methodName = "Write" + propertyType.Name;
                }

                sourceBuilder.AppendLine(
                    $$"""
                                if (syntaxNode.{{propertyName}} != default({{propertyType.Name}}))
                                {
                                    var {{camelCaseName}}Builder = new StringBuilder();
                                    {{methodName}}({{camelCaseName}}Builder, syntaxNode.{{propertyName}});
                                    properties.Add($"\"{{camelCaseName}}\":{{{camelCaseName}}Builder.ToString()}");
                                }
                    """
                );
            }
            else if (
                (
                    propertyType.IsGenericType
                    && (
                        propertyType
                            .ConstructedFrom.ToDisplayString()
                            .StartsWith("Microsoft.CodeAnalysis.SyntaxList<")
                        || propertyType
                            .ConstructedFrom.ToDisplayString()
                            .StartsWith("Microsoft.CodeAnalysis.SeparatedSyntaxList<")
                    )
                )
                || propertyType.Name == nameof(SyntaxTokenList)
                || propertyType.Name == nameof(SyntaxTriviaList)
            )
            {
                var methodName = "WriteSyntaxNode";
                if (propertyType.Name == nameof(SyntaxTokenList))
                {
                    methodName = "WriteSyntaxToken";
                }
                else if (propertyType.Name == nameof(SyntaxTriviaList))
                {
                    methodName = "WriteSyntaxTrivia";
                }
                else
                {
                    var genericArgument = propertyType.TypeArguments[0];
                    if (!genericArgument.IsAbstract)
                    {
                        methodName = "Write" + genericArgument.Name;
                    }
                }

                sourceBuilder.AppendLine(
                    $$"""
                                var {{camelCaseName}} = new List<string>();
                                foreach(var node in syntaxNode.{{propertyName}})
                                {
                                    var innerBuilder = new StringBuilder();
                                    {{methodName}}(innerBuilder, node);
                                    {{camelCaseName}}.Add(innerBuilder.ToString());
                                }
                                properties.Add($"\"{{camelCaseName}}\":[{string.Join(",", {{camelCaseName}})}]");
                    """
                );
            }
            else
            {
                this.missingTypes.Add(
                    PadToSize(type.Name + "." + propertyName + ": ", 40) + propertyType
                );
            }
        }

        sourceBuilder.AppendLine(
            """
                        builder.Append(string.Join(",", properties.Where(o => o != null)));
                        builder.Append("}");
                    }
            """
        );
    }

    private static string CamelCaseName(string name)
    {
        return name.ToLower(CultureInfo.InvariantCulture)[0] + name[1..];
    }

    private static string PadToSize(string value, int size)
    {
        while (value.Length < size)
        {
            value += " ";
        }

        return value;
    }
}
