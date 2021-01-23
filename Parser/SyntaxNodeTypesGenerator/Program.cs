using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace SyntaxNodeTypesGenerator
{
    [TestFixture]
    public class Program
    {
        static List<string> missingTypes = new List<string>();

        [TestCase]
        public void DoWork()
        {
            var rootRepositoryFolder = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (rootRepositoryFolder.Name != "Parser")
            {
                rootRepositoryFolder = rootRepositoryFolder.Parent;
            }

            rootRepositoryFolder = rootRepositoryFolder.Parent;

            var syntaxNodeTypes = typeof(CompilationUnitSyntax).Assembly.GetTypes()
                .Where(o => !o.IsAbstract && typeof(CSharpSyntaxNode).IsAssignableFrom(o)).ToList();

            var fileName = rootRepositoryFolder.FullName + @"\prettier-plugin-csharpier\src\Printer\NodeTypes.ts";

            using (var file = new StreamWriter(fileName, false))
            {
                foreach (var syntaxNodeType in syntaxNodeTypes)
                {
                    GenerateNode(file, syntaxNodeType);
                }
            }

            if (missingTypes.Any())
            {
                throw new Exception(Environment.NewLine + string.Join(Environment.NewLine, missingTypes));
            }
        }

        private static void GenerateNode(StreamWriter file, Type syntaxNodeType)
        {
            if (syntaxNodeType != typeof(CompilationUnitSyntax))
            {
                return;
            }
            file.WriteLine($"export interface ${TypescriptInterfaceName(syntaxNodeType)} extends SyntaxTreeNode<\"TypescriptInterfaceName(syntaxNodeType)\"> {");
            file.WriteLine("}");
        }

        private static string TypescriptInterfaceName(Type syntaxNodeType)
        {
            var name = syntaxNodeType.Name;
            if (name.EndsWith("Syntax"))
            {
                name = name.Substring(0, name.Length - "Syntax".Length);
            }

            return name + "Node";
        }
    }
}