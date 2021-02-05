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
            var rootDirectory = new DirectoryInfo(@"C:\Projects\csharpier");
            var output = new StringBuilder();

            output.AppendLine(@"using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        public Doc Print(SyntaxNode syntaxNode)
        {");

            var csharpDirectory = Path.Combine(rootDirectory.FullName, @"CSharpier\CSharpier\Printer");
            var isFirst = true;
            foreach (var file in new DirectoryInfo(csharpDirectory).GetFiles())
            {
                output.Append("            ");
                output.Append(isFirst ? "if" : "else if");
                isFirst = false;
                var name = file.Name.Replace(".cs", "");
                var camelCaseName = name[0].ToString().ToLower() + name.Substring(1);
                output.AppendLine($@" (syntaxNode is {name} {camelCaseName})
            {{
                return this.Print{name}({camelCaseName});
            }}");
            }

            output.AppendLine(@"
            throw new Exception(""Can't handle "" + syntaxNode.GetType().Name);
        }
    }
}");

            File.WriteAllText(csharpDirectory + ".generated.cs", output.ToString());
        }
    }
}