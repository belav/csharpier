using System.IO;
using System.Text;
using NUnit.Framework;

namespace Worker
{
    public class GeneratePrinterSwitch
    {
        [Test]
        public void DoWork()
        {
            var rootDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (rootDirectory.Name != "Src")
            {
                rootDirectory = rootDirectory.Parent;
            }
            var output = new StringBuilder();

            output.AppendLine(@"using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        public Doc Print(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                return null;
            }

            switch (syntaxNode)
            {");

            var csharpDirectory = Path.Combine(rootDirectory.FullName, @"CSharpier.Core\Printer");
            foreach (var file in new DirectoryInfo(csharpDirectory).GetFiles())
            {
                var name = file.Name.Replace(".cs", "");
                var camelCaseName = name[0].ToString().ToLower() + name.Substring(1);
                output.AppendLine($@"                case {name} {camelCaseName}:
                    return this.Print{name}({camelCaseName});");
            }

            output.AppendLine(@"
                default:
                    throw new Exception(""Can't handle "" + syntaxNode.GetType().Name);
            }
        }
    }
}");

            File.WriteAllText(csharpDirectory + ".generated.cs", output.ToString());
        }
    }
}