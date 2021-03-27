using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Worker
{
    public class MissingTypeChecker
    {
        [Test]
        [Ignore("This is used to produce a list of SyntaxNodes that do not exist in CSharpier/Printer/*.cs")]
        public void DoWork()
        {
            // See https://github.com/belav/csharpier/issues/29 for details about what was missing as of c#9
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory.Name != "Src")
            {
                directory = directory.Parent;
            }

            var files = Directory.GetFiles(
                    Path.Combine(directory.FullName, "CSharpier/Printer")
                )
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();

            var syntaxNodeTypes = typeof(CompilationUnitSyntax).Assembly.GetTypes()
                .Where(
                    o => !o.IsAbstract
                    && typeof(CSharpSyntaxNode).IsAssignableFrom(o)
                )
                .ToList();

            var missingTypes = new List<Type>();

            foreach (var nodeType in syntaxNodeTypes)
            {
                if (!files.Contains(nodeType.Name))
                {
                    missingTypes.Add(nodeType);
                }
            }

            foreach (var missingType in missingTypes.OrderBy(o => o.Name))
            {
                Console.WriteLine(missingType.Name);
            }
        }
    }
}
