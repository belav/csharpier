using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.TestFileTests
{
    public class BaseTest
    {
        private DirectoryInfo rootDirectory;

        [SetUp]
        public void Setup()
        {
            this.rootDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (this.rootDirectory.Name != "CSharpier.Tests")
            {
                this.rootDirectory = this.rootDirectory.Parent;
            }
        }
        
        public void RunTest(string folderName, string fileName)
        {
            var filePath = Path.Combine(this.rootDirectory.FullName, "TestFiles", folderName, fileName + ".cst");
            var code = File.ReadAllText(filePath);
            
            var formatter = new CodeFormatter();
            var result = formatter.Format(code, new Options
            {
                IncludeDocTree = true,
                IncludeAST = true
            });

            var docTreePath = filePath.Replace(".cst", ".doctree.txt");
            File.WriteAllText(docTreePath, result.DocTree, Encoding.UTF8);

            var astPath = filePath.Replace(".cst", ".json");
            File.WriteAllText(astPath, result.AST, Encoding.UTF8);
            
            var actualFilePath = filePath.Replace(".cst", ".actual.cst");
            File.WriteAllText(actualFilePath, result.Code, Encoding.UTF8);

            result.Code.Should().Be(code);
        }
    }
}