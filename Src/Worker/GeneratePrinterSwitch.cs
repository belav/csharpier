using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Worker
{
    public class GeneratePrinterSwitch
    {
        [Test]
        public void DoWork()
        {
            var rootDirectory = new DirectoryInfo(
                Directory.GetCurrentDirectory()
            );
            while (rootDirectory.Name != "Src")
            {
                rootDirectory = rootDirectory.Parent;
            }
            var output = new StringBuilder();

            output.AppendLine(
                @"
using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class Node
    {
        [ThreadStatic]
        private static int depth;

        public static Doc Print(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                return Doc.Null;
            }

            // TODO 0 kill? runtime repo has files that will fail on deep recursion
            if (depth > 200)
            {
                throw new InTooDeepException();
            }

            depth++;
            try
            {
                switch (syntaxNode)
                {"
            );

            var nodes = new List<string>();
            var newFiles = Path.Combine(
                rootDirectory.FullName,
                "CSharpier/SyntaxPrinter/SyntaxNodePrinters"
            );
            foreach (var file in new DirectoryInfo(newFiles).GetFiles())
            {
                nodes.Add(file.Name.Replace(".cs", string.Empty) + "Syntax");
            }

            foreach (var name in nodes.OrderBy(o => o))
            {
                var camelCaseName =
                    name[0].ToString().ToLower() + name.Substring(1);
                output.AppendLine(
                    $"                    case {name} {camelCaseName}:"
                );
                output.AppendLine(
                    $"                        return {name.Replace("Syntax", string.Empty)}.Print({camelCaseName});"
                );
            }

            output.AppendLine(
                @"
                    default:
                        throw new Exception(""Can't handle "" + syntaxNode.GetType().Name);
                }
            }
            finally
            {
                depth--;
            }
        }
    }
}"
            );

            File.WriteAllText(
                Path.Combine(
                    rootDirectory.FullName,
                    "CSharpier/SyntaxPrinter/Node.cs"
                ),
                output.ToString()
            );
        }
    }
}
