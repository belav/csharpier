using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Worker
{
    [TestFixture]
    public class SyntaxNodeComparerGenerator
    {
        [Test]
        [Ignore("Run manually to update SyntaxNodeComparerGenerator.generated.cs. Then run csharpier on the result")]
        public void DoWork()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory.Name != "Src")
            {
                directory = directory.Parent;
            }

            var syntaxNodeTypes = typeof(CompilationUnitSyntax).Assembly.GetTypes()
                .Where(
                    o => !o.IsAbstract
                    && typeof(CSharpSyntaxNode).IsAssignableFrom(o)
                )
                .ToList();

            var fileName =
                directory.FullName + "/CSharpier/SyntaxNodeComparer.generated.cs";
            using (var file = new StreamWriter(fileName, false))
            {
                file.WriteLine(
                    @"#pragma warning disable CS0168
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class SyntaxNodeComparer
    {
        private CompareResult Compare(SyntaxNode originalNode, SyntaxNode formattedNode)
        {
            if (originalNode == null && formattedNode == null)
            {
                return Equal;
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
            {"
                );
                foreach (var syntaxNodeType in syntaxNodeTypes)
                {
                    var lowerCaseName =
                        syntaxNodeType.Name[0].ToString()
                            .ToLower() + syntaxNodeType.Name.Substring(1);
                    file.WriteLine($@"                case {syntaxNodeType.Name} {lowerCaseName}:
                    return this.Compare{syntaxNodeType.Name}({lowerCaseName}, formattedNode as {syntaxNodeType.Name});");
                }

                file.WriteLine(
                    @"                default:
                    throw new Exception(""Can't handle "" + originalNode.GetType().Name);
            }
        }
        "
                );

                foreach (var syntaxNodeType in syntaxNodeTypes)
                {
                    GenerateMethod(file, syntaxNodeType);
                }

                file.WriteLine("    }");
                file.WriteLine("}");
            }
        }

        private void GenerateMethod(StreamWriter file, Type type)
        {
            file.WriteLine(@$"        private CompareResult Compare{type.Name}({type.Name} originalNode, {type.Name} formattedNode)
        {{
            CompareResult result;");

            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyName = propertyInfo.Name;

                if (
                    propertyName == "Language"
                    || propertyName == "Parent"
                    || propertyName == "HasLeadingTrivia" // we modify/remove whitespace and new lines so we can't look at these properties.
                    || propertyName == "HasTrailingTrivia"
                    || propertyName == "ParentTrivia"
                    || propertyName == "Arity"
                    || propertyName == "SpanStart"
                )
                {
                    continue;
                }

                var camelCaseName = CamelCaseName(propertyName);
                var propertyType = propertyInfo.PropertyType;

                if (
                    Ignored.Properties.Contains(camelCaseName)
                    || Ignored.Types.Contains(propertyType)
                    || (Ignored.PropertiesByType.ContainsKey(type)
                    && Ignored.PropertiesByType[type].Contains(camelCaseName))
                )
                {
                    continue;
                }

                if (propertyType == typeof(bool) || propertyType == typeof(Int32))
                {
                    file.WriteLine($@"            if (originalNode.{propertyName} != formattedNode.{propertyName}) return NotEqual(originalNode, formattedNode);");
                }
                else if (propertyType == typeof(SyntaxToken))
                {
                    file.WriteLine($"            result = this.Compare(originalNode.{propertyName}, formattedNode.{propertyName}, originalNode, formattedNode);");
                    file.WriteLine($"            if (result.IsInvalid) return result;");
                }
                else if (propertyType == typeof(SyntaxTrivia))
                {
                    file.WriteLine($"            result = this.Compare(originalNode.{propertyName}, formattedNode.{propertyName});");
                    file.WriteLine($"            if (result.IsInvalid) return result;");
                }
                else if (
                    typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType)
                )
                {
                    file.WriteLine($"            originalStack.Push(originalNode.{propertyName});");
                    file.WriteLine($"            formattedStack.Push(formattedNode.{propertyName});");
                }
                else if (
                    propertyType == typeof(SyntaxTokenList)
                    || (propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>))
                )
                {
                    var compare = propertyType == typeof(SyntaxTokenList)
                        ? "Compare"
                        : "null";
                    file.WriteLine($"            result = this.CompareLists(originalNode.{propertyName}, formattedNode.{propertyName}, {compare}, o => o.Span, originalNode.Span, formattedNode.Span);");
                    file.WriteLine($"            if (result.IsInvalid) return result;");
                }
                else if (
                    propertyType.IsGenericType
                    && propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>)
                )
                {
                    file.WriteLine($"            result = this.CompareLists(originalNode.{propertyName}, formattedNode.{propertyName}, null, o => o.Span, originalNode.Span, formattedNode.Span);");
                    file.WriteLine($"            if (result.IsInvalid) return result;");

                    file.WriteLine($"            result = this.CompareLists(originalNode.{propertyName}.GetSeparators().ToList(), formattedNode.{propertyName}.GetSeparators().ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);");
                    file.WriteLine($"            if (result.IsInvalid) return result;");
                }
            }
            file.WriteLine("            return Equal;");
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
