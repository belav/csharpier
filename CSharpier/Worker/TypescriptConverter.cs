using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Worker
{
    public class TypescriptConverter
    {
        [Test]
        public void DoWork()
        {
            return;
            var rootDirectory = new DirectoryInfo(@"C:\Projects\csharpier");
            var typescriptTypeDirectory = Path.Combine(rootDirectory.FullName, @"prettier-plugin-csharpier\src\Printer");
            var inputFile = Path.Combine(typescriptTypeDirectory, "PrintAttributeLists.ts");

            var nodeNameWithoutSyntax = "AttributeStuff";
            var nodeNameWithSyntax = nodeNameWithoutSyntax + "Syntax";

            using var streamReader = new StreamReader(inputFile);
            var output = new StringBuilder();
            output.AppendLine(@$"using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{{
    public partial class Printer
    {{
        private Doc Print{nodeNameWithSyntax}({nodeNameWithSyntax} node)
        {{");
            var foundMethod = false;
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.StartsWith("export const print"))
                {
                    foundMethod = true;
                }

                if (!line.StartsWith("    ") || !foundMethod)
                {
                    continue;
                }

                if (line == "    const node = path.getValue();")
                {
                    continue;
                }

                line = "        " + line;

                line = Regex.Replace(line, @"path.call\(print, ""(\w+)""\)", "this.Print(node.$1)");
                line = Regex.Replace(line, @"path.map\(print, ""(\w+)""\)", "node.$1.Select(this.Print)");
                line = Regex.Replace(line, @"path.call\(o => print(\w+)\(o, options, print\), ""(\w+)""\)", "this.Print$1Syntax(node.$2)");
                line = Regex.Replace(line, @"path.map\(\w+ => print(\w+)\(\w+, options, print\), ""(\w+)""\)", "node.$2.Select(this.Print$1Syntax)");
                line = Regex.Replace(line, @"printPathSyntaxToken\(path, ""(\w+)""\)", "String(node.$1.Text)");
                line = Regex.Replace(line, @"node\.(\w+)", o => "node." + o.Groups[1].Value[0].ToString().ToUpper() + o.Groups[1].Value.Substring(1));
                line = Regex.Replace(line, @"concat\(\[(.+)\]\)", "Concat($1)");
                line = Regex.Replace(line, @"concat\((.+)\)", "Concat($1)");
                line = Regex.Replace(line, @"""([^""|.]+)""", "String(\"$1\")");
                line = Regex.Replace(line, @"if \(node.(\w+)\)", "if (NotNull(node.$1))");
                line = Regex.Replace(line, @"const (\w+): Doc\[\] \= \[\]", "var $1 = new Parts()");
                line = line.Replace("printPathIdentifier(path)", "node.Identifier.Text");
                line = line.Replace("printModifiers(node", "this.PrintModifiers(node.Modifiers");
                line = line.Replace("printExtraNewLines(node, parts, ", "//this.PrintExtraNewLines(node, ");
                line = line.Replace("printAttributeLists(node, parts, path, options, print)", "this.PrintAttributeLists(node.AttributeLists, parts)");
                line = line.Replace("innerParts.splice(innerParts.length - 1, 1);", "innerParts.TheParts.RemoveAt(innerParts.TheParts.Count - 1);");
                line = line.Replace(".Add(", ".Add(");
                line = line.Replace("group(", "Group(");
                line = line.Replace("indent(", "Indent(");
                line = line.Replace("join(", "Join(");
                line = line.Replace("hardline", "HardLine");
                line = line.Replace("softline", "SoftLine");
                line = line.Replace("line", "Line");
                line = line.Replace("const ", "var ");
                line = line.Replace(".length", ".Count");

                output.AppendLine(line);
            }

            output.AppendLine(@"        }
    }
}");

            var csharpDirectory = Path.Combine(rootDirectory.FullName, @"CSharpier\CSharpier\Printer");
            var newFile = Path.Combine(csharpDirectory, nodeNameWithSyntax + ".cs");
            if (File.Exists(newFile))
            {
                throw new Exception("We no longer want to overwrite files!!");
            }
            File.WriteAllText(newFile, output.ToString());
        }
    }
}