using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace SyntaxNodeJsonWriterGenerator
{
    [TestFixture]
    public class Generator
    {
        List<string> missingTypes = new List<string>();

        [Test]
        public void DoWork()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory.Name != "CSharpier")
            {
                directory = directory.Parent;
            }

            var syntaxNodeTypes = typeof(CompilationUnitSyntax).Assembly.GetTypes()
                .Where(o => !o.IsAbstract && typeof(CSharpSyntaxNode).IsAssignableFrom(o)).ToList();

            var fileName = directory.FullName + @"\CSharpier.Parser\SyntaxNodeJsonWriter.generated.cs";
            using (var file = new StreamWriter(fileName, false))
            {
                file.WriteLine("using System.Collections.Generic;");
                file.WriteLine("using System.IO;");
                file.WriteLine("using System.Linq;");
                file.WriteLine("using System.Text;");
                file.WriteLine("using Microsoft.CodeAnalysis;");
                file.WriteLine("using Microsoft.CodeAnalysis.CSharp;");
                file.WriteLine("using Microsoft.CodeAnalysis.CSharp.Syntax;");
                file.WriteLine();
                file.WriteLine("namespace Parser");
                file.WriteLine("{");
                file.WriteLine("    public partial class SyntaxNodeJsonWriter");
                file.WriteLine("    {");

                file.WriteLine(
                    $"        public static void WriteSyntaxNode(StringBuilder builder, SyntaxNode syntaxNode)");
                file.WriteLine("        {");
                foreach (var syntaxNodeType in syntaxNodeTypes)
                {
                    file.WriteLine(
                        $"            if (syntaxNode is {syntaxNodeType.Name}) Write{syntaxNodeType.Name}(builder, syntaxNode as {syntaxNodeType.Name});");
                }

                file.WriteLine("        }");
                file.WriteLine("");

                foreach (var syntaxNodeType in syntaxNodeTypes)
                {
                    GenerateMethod(file, syntaxNodeType);
                }

                GenerateMethod(file, typeof(SyntaxToken));
                GenerateMethod(file, typeof(SyntaxTrivia));

                file.WriteLine("    }");
                file.WriteLine("}");
            }

            File.Copy(fileName, fileName.Replace("CSharpier.Parser", "CSharpier"), true);
            
            if (missingTypes.Any())
            {
                throw new Exception(Environment.NewLine + string.Join(Environment.NewLine, missingTypes));
            }
        }

        private void GenerateMethod(StreamWriter file, Type type)
        {
            file.WriteLine(
                $"        public static void Write{type.Name}(StringBuilder builder, {type.Name} syntaxNode)");
            file.WriteLine("        {");
            file.WriteLine("            builder.Append(\"{\");");
            file.WriteLine("            var properties = new List<string>();");
            file.WriteLine(
                $"            properties.Add($\"\\\"nodeType\\\":\\\"{{GetNodeType(syntaxNode.GetType())}}\\\"\");");
            file.WriteLine($"            properties.Add($\"\\\"kind\\\":\\\"{{syntaxNode.Kind().ToString()}}\\\"\");");

            if (type == typeof(SyntaxTrivia))
            {
                file.WriteLine($"            if (syntaxNode.RawKind == 8541 || syntaxNode.RawKind == 8542)");
                file.WriteLine($"            {{");
                file.WriteLine(
                    $"                properties.Add(WriteString(\"commentText\", syntaxNode.ToString()));");
                file.WriteLine($"            }}");
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyName = propertyInfo.Name;
                var camelCaseName = CamelCaseName(propertyName);
                var propertyType = propertyInfo.PropertyType;

                if (Ignored.Properties.Contains(camelCaseName)
                    || Ignored.Types.Contains(propertyType)
                    || (Ignored.PropertiesByType.ContainsKey(type) &&
                        Ignored.PropertiesByType[type].Contains(camelCaseName)))
                {
                    continue;
                }

                if (propertyType == typeof(bool))
                {
                    file.WriteLine(
                        $"            properties.Add(WriteBoolean(\"{camelCaseName}\", syntaxNode.{propertyName}));");
                }
                else if (propertyType == typeof(string))
                {
                    file.WriteLine(
                        $"            properties.Add(WriteString(\"{camelCaseName}\", syntaxNode.{propertyName}));");
                }
                else if (propertyType == typeof(int))
                {
                    file.WriteLine(
                        $"            properties.Add(WriteInt(\"{camelCaseName}\", syntaxNode.{propertyName}));");
                }
                else if (typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType)
                         || propertyType == typeof(SyntaxToken)
                         || propertyType == typeof(SyntaxTrivia))
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

                    file.WriteLine($"            if (syntaxNode.{propertyName} != default({propertyType.Name}))");
                    file.WriteLine("            {");
                    file.WriteLine($"                var {camelCaseName}Builder = new StringBuilder();");
                    file.WriteLine($"                {methodName}({camelCaseName}Builder, syntaxNode.{propertyName});");
                    file.WriteLine(
                        $"                properties.Add($\"\\\"{camelCaseName}\\\":{{{camelCaseName}Builder.ToString()}}\");");
                    file.WriteLine("            }");
                }
                else if ((propertyType.IsGenericType &&
                          (propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>) ||
                           propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>)))
                         || propertyType == typeof(SyntaxTokenList)
                         || propertyType == typeof(SyntaxTriviaList))
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

                    file.WriteLine($"            var {camelCaseName} = new List<string>();");
                    file.WriteLine($"            foreach(var node in syntaxNode.{propertyName})");
                    file.WriteLine("            {");
                    file.WriteLine($"                var innerBuilder = new StringBuilder();");
                    file.WriteLine($"                {methodName}(innerBuilder, node);");
                    file.WriteLine($"                {camelCaseName}.Add(innerBuilder.ToString());");
                    file.WriteLine("            }");
                    file.WriteLine(
                        $"            properties.Add($\"\\\"{camelCaseName}\\\":[{{string.Join(\",\", {camelCaseName})}}]\");");
                }
                else
                {
                    missingTypes.Add(PadToSize(type.Name + "." + propertyName + ": ", 40) +
                                     propertyType);
                }
            }

            file.WriteLine("            builder.Append(string.Join(\",\", properties.Where(o => o != null)));");
            file.WriteLine("            builder.Append(\"}\");");
            file.WriteLine("        }");
        }

        private static string CamelCaseName(string name)
        {
            return name.ToLower()[0] + name.Substring(1);
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
}