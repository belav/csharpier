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
    public class TypescriptNodeTypeGenerator
    {
        static List<string> missingTypes = new List<string>();

        [TestCase]
        public void DoWork()
        {
            var rootRepositoryFolder = new DirectoryInfo(
                Directory.GetCurrentDirectory());
            while (rootRepositoryFolder.Name != "Src")
            {
                rootRepositoryFolder = rootRepositoryFolder.Parent;
            }

            rootRepositoryFolder = rootRepositoryFolder.Parent;

            var syntaxNodeTypes = typeof(CompilationUnitSyntax).Assembly.GetTypes().Where(
                o => !o.IsAbstract &&
                typeof(CSharpSyntaxNode).IsAssignableFrom(o)).OrderBy(
                o => o.Name).ToList();

            var fileName = rootRepositoryFolder.FullName + "/prettier-plugin-csharpier/src/Printer/NodeTypes.ts";

            using (var file = new StreamWriter(fileName, false))
            {
                file.WriteLine(
                    "import { SyntaxToken, SyntaxTreeNode } from \"./SyntaxTreeNode\";");
                file.WriteLine();

                foreach (var syntaxNodeType in syntaxNodeTypes)
                {
                    GenerateNode(file, syntaxNodeType);
                }
            }

            if (missingTypes.Any())
            {
                throw new Exception(
                    Environment.NewLine + string.Join(
                        Environment.NewLine,
                        missingTypes));
            }
        }

        private static void GenerateNode(StreamWriter file, Type type)
        {
            file.WriteLine($"interface {TypescriptInterfaceName(type)} extends SyntaxTreeNode<\"{TypescriptNodeType(type)}\"> {{");
            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyName = propertyInfo.Name;
                var camelCaseName = CamelCaseName(propertyName);
                var propertyType = propertyInfo.PropertyType;

                if (
                    Ignored.Properties.Contains(camelCaseName) ||
                    Ignored.Types.Contains(propertyType) ||
                    (Ignored.PropertiesByType.ContainsKey(type) &&
                    Ignored.PropertiesByType[type].Contains(camelCaseName)) ||
                    IsBaseClassProperty(camelCaseName)
                )
                {
                    continue;
                }

                var typescriptType = "any";
                var nullable = true;
                if (propertyType == typeof(bool))
                {
                    typescriptType = "boolean";
                }
                else if (propertyType == typeof(Int32))
                {
                    typescriptType = "number";
                }
                else if (propertyType == typeof(SyntaxToken))
                {
                    typescriptType = "SyntaxToken";
                }
                else if (propertyType == typeof(SyntaxTrivia))
                {
                    typescriptType = "SyntaxTrivia";
                }
                else if (
                    typeof(CSharpSyntaxNode).IsAssignableFrom(propertyType)
                )
                {
                    typescriptType = "SyntaxTreeNode";
                    if (!propertyType.IsAbstract)
                    {
                        typescriptType = TypescriptInterfaceName(propertyType);
                    }
                }
                else if (
                    (propertyType.IsGenericType &&
                    (propertyType.GetGenericTypeDefinition() == typeof(SyntaxList<>) ||
                    propertyType.GetGenericTypeDefinition() == typeof(SeparatedSyntaxList<>))) ||
                    propertyType == typeof(SyntaxTokenList) ||
                    propertyType == typeof(SyntaxTriviaList)
                )
                {
                    nullable = false;
                    if (propertyType == typeof(SyntaxTokenList))
                    {
                        typescriptType = "SyntaxToken";
                    }
                    else if (propertyType == typeof(SyntaxTriviaList))
                    {
                        typescriptType = "SyntaxTrivia";
                    }
                    else
                    {
                        typescriptType = "SyntaxTreeNode";

                        var genericArgument = propertyType.GetGenericArguments()[
                            0
                        ];
                        if (!genericArgument.IsAbstract)
                        {
                            typescriptType = TypescriptInterfaceName(
                                genericArgument);
                        }
                    }

                    typescriptType += "[]";
                }
                else
                {
                    missingTypes.Add(
                        PadToSize(type.Name + "." + propertyName + ": ", 40) + propertyType);
                }

                file.WriteLine($"    {CamelCaseName(propertyInfo.Name)}{(nullable ? "?" : "")}: {typescriptType};");
            }

            file.WriteLine("}");
            file.WriteLine("");
        }

        private static bool IsBaseClassProperty(string camelCaseName)
        {
            return camelCaseName == "isMissing" ||
            camelCaseName == "isStructuredTrivia" ||
            camelCaseName == "hasStructuredTrivia" ||
            camelCaseName == "containsSkippedText" ||
            camelCaseName == "containsDirectives" ||
            camelCaseName == "containsDiagnostics" ||
            camelCaseName == "hasLeadingTrivia" ||
            camelCaseName == "hasTrailingTrivia" ||
            camelCaseName == "containsAnnotations";
        }

        private static string CamelCaseName(string value)
        {
            return value.Substring(0, 1).ToLower() + value.Substring(1);
        }

        private static string TypescriptNodeType(Type syntaxNodeType)
        {
            var name = syntaxNodeType.Name;
            if (name.EndsWith("Syntax"))
            {
                name = name.Substring(0, name.Length - "Syntax".Length);
            }

            return name;
        }

        private static string TypescriptInterfaceName(Type syntaxNodeType)
        {
            return TypescriptNodeType(syntaxNodeType) + "Node";
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
