using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.FakeGenerators;

public class SyntaxNodeJsonWriterGenerator
{
    private readonly List<string> missingTypes = [];

    // this would probably be easier to understand as a scriban template but is a lot of effort
    // to switch and doesn't really change at this point
    public void Execute(CodeContext context)
    {
        context.AddSource("SyntaxNodeJsonWriter.generated", this.GenerateSource());
    }

    private string GenerateSource()
    {
        var syntaxNodeTypes = ValidNodeTypes.Get();

        var sourceBuilder = new StringBuilder();
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
        foreach (var syntaxNodeType in syntaxNodeTypes)
        {
            sourceBuilder.AppendLine(
                CultureInfo.InvariantCulture,
                $"            if (syntaxNode is {syntaxNodeType.Name}) Write{syntaxNodeType.Name}(builder, syntaxNode as {syntaxNodeType.Name});"
            );
        }

        sourceBuilder.AppendLine("        }");
        sourceBuilder.AppendLine();

        foreach (var syntaxNodeType in syntaxNodeTypes)
        {
            this.GenerateMethod(sourceBuilder, syntaxNodeType);
        }

        this.GenerateMethod(sourceBuilder, typeof(SyntaxToken));
        this.GenerateMethod(sourceBuilder, typeof(SyntaxTrivia));

        sourceBuilder.AppendLine("    }");
        sourceBuilder.AppendLine("}");

        if (this.missingTypes.Count != 0)
        {
            throw new Exception(
                Environment.NewLine + string.Join(Environment.NewLine, this.missingTypes)
            );
        }

        return sourceBuilder.ToString();
    }

    private void GenerateMethod(StringBuilder sourceBuilder, Type type)
    {
        sourceBuilder.AppendLine(
            CultureInfo.InvariantCulture,
            $$"""
                    public static void Write{{type.Name}}(StringBuilder builder, {{type.Name}} syntaxNode)
                    {
                        builder.Append("{");
                        var properties = new List<string>();
                        properties.Add($"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\"");
                        properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            """
        );

        if (type == typeof(SyntaxTrivia))
        {
            sourceBuilder.AppendLine(
                "            properties.Add(WriteString(\"text\", syntaxNode.ToString()));"
            );
        }

        foreach (var propertyInfo in type.GetProperties().OrderBy(o => o.Name))
        {
            var propertyName = propertyInfo.Name;
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

            if (propertyType == typeof(bool))
            {
                sourceBuilder.AppendLine(
                    CultureInfo.InvariantCulture,
                    $"            properties.Add(WriteBoolean(\"{camelCaseName}\", syntaxNode.{propertyName}));"
                );
            }
            else if (propertyType == typeof(string))
            {
                sourceBuilder.AppendLine(
                    CultureInfo.InvariantCulture,
                    $"            properties.Add(WriteString(\"{camelCaseName}\", syntaxNode.{propertyName}));"
                );
            }
            else if (propertyType == typeof(int))
            {
                sourceBuilder.AppendLine(
                    CultureInfo.InvariantCulture,
                    $"            properties.Add(WriteInt(\"{camelCaseName}\", syntaxNode.{propertyName}));"
                );
            }
            else if (
                typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType)
                || propertyType == typeof(SyntaxToken)
                || propertyType == typeof(SyntaxTrivia)
            )
            {
                var methodName = "WriteSyntaxNode";
                if (propertyType == typeof(SyntaxToken))
                {
                    methodName = "WriteSyntaxToken";
                }
                else if (propertyType == typeof(SyntaxTrivia))
                {
                    methodName = "WriteSyntaxTrivia";
                }
                else if (!propertyType.IsAbstract)
                {
                    methodName = "Write" + propertyType.Name;
                }

                sourceBuilder.AppendLine(
                    CultureInfo.InvariantCulture,
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
                        propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>)
                        || propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>)
                    )
                )
                || propertyType == typeof(SyntaxTokenList)
                || propertyType == typeof(SyntaxTriviaList)
            )
            {
                var methodName = "WriteSyntaxNode";
                if (propertyType == typeof(SyntaxTokenList))
                {
                    methodName = "WriteSyntaxToken";
                }
                else if (propertyType == typeof(SyntaxTriviaList))
                {
                    methodName = "WriteSyntaxTrivia";
                }
                else
                {
                    var genericArgument = propertyType.GetGenericArguments()[0];
                    if (!genericArgument.IsAbstract)
                    {
                        methodName = "Write" + genericArgument.Name;
                    }
                }

                sourceBuilder.AppendLine(
                    CultureInfo.InvariantCulture,
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
