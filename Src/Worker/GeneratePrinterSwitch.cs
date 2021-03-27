using System.IO;
using System.Text;
using NUnit.Framework;

namespace Worker
{
    public class GeneratePrinterSwitch
    {
        [Test]
        [Ignore(
                "Run this manually if you need to regenerate the Printer.generated.cs file. Then run csharpier on the result")]
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
                @"using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private int depth = 0;
        public Doc Print(SyntaxNode syntaxNode)
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
            try {
                switch (syntaxNode)
                {"
            );

            var csharpDirectory = Path.Combine(
                rootDirectory.FullName,
                "CSharpier/Printer"
            );
            foreach (var file in new DirectoryInfo(csharpDirectory).GetFiles())
            {
                var name = file.Name.Replace(".cs", "");
                var camelCaseName =
                    name[0].ToString().ToLower() + name.Substring(1);
                output.AppendLine(
                    $@"                    case {name} {camelCaseName}:
                        return this.Print{name}({camelCaseName});"
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
                csharpDirectory + ".generated.cs",
                output.ToString()
            );
        }
    }
}
