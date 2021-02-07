using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class Samples
    {
        [Test]
        public void Scratch()
        {
            this.RunTest("Scratch");
        }

        [Test]
        public void AllInOne()
        {
            this.RunTest("AllInOne");
        }

        public void RunTest(string fileName)
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory.Name != "CSharpier.Tests")
            {
                directory = directory.Parent;
            }

            var file = Path.Combine(directory.FullName, $"Samples/{fileName}.cst");
            var code = File.ReadAllText(file);
            var stopwatch = Stopwatch.StartNew();
            var result = new CodeFormatter().Format(code, new Options
            {
                IncludeDocTree = true,
                IncludeAST = true,
            });
            Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            
            // TODO what about BOM? keep it if incoming file?
            File.WriteAllText(file.Replace(".cst", ".Formatted.cst"), result.Code, new UTF8Encoding(false));
            File.WriteAllText(file.Replace(".cst", ".doctree.txt"), result.DocTree, new UTF8Encoding(false));
            File.WriteAllText(file.Replace(".cst", ".json"), result.AST, new UTF8Encoding(false));
        }
    }
}