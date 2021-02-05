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
            
            var formatter = new Formatter();
            var docTree = formatter.Format(code, new Options { PrintDocTree = true });
            var docTreePath = filePath.Replace(".cst", ".doctree.txt");
            File.WriteAllText(docTreePath, docTree);
            
            var actualCode = formatter.Format(code, new Options());

            var actualFilePath = filePath.Replace(".cst", ".actual.cst");
            File.WriteAllText(actualFilePath, actualCode, Encoding.UTF8);

            actualCode.Should().Be(code);
        }
    }
}