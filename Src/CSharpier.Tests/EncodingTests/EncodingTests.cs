using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using UtfUnknown;

namespace CSharpier.Tests.EncodingTests
{
    [TestFixture]
    public class EncodingTests
    {
        private DirectoryInfo rootDirectory;

        [SetUp]
        public void Setup()
        {
            this.rootDirectory = new DirectoryInfo(
                Directory.GetCurrentDirectory());
            while (this.rootDirectory.Name != "CSharpier.Tests")
            {
                this.rootDirectory = this.rootDirectory.Parent;
            }
        }
        
        [Test]
        public void UTF8()
        {
            this.RunTest(nameof(UTF8));
        }
        
        [Test]
        public void UTF8BOM()
        {
            this.RunTest(nameof(UTF8BOM));
        }
        
        [Test]
        public void USC2LEBOM()
        {
            this.RunTest(nameof(USC2LEBOM));
        }
        
        public void RunTest(string fileName)
        {
            var filePath = Path.Combine(
                this.rootDirectory.FullName,
                "EncodingTests",
                fileName + ".cst");
            using var reader = new StreamReader(filePath, Encoding.UTF8, true);
            var code = reader.ReadToEnd();

            var detectionResult = CharsetDetector.DetectFromFile(filePath);

            var encoding = detectionResult.Detected.Encoding;
            reader.Close();

            var formatter = new CodeFormatter();
            var result = formatter.Format(code, new Options());

            var actualFilePath = filePath.Replace(".cst", ".actual.cst");
            using var stream = File.Open(actualFilePath, FileMode.Create);
            using var writer = new StreamWriter(stream, encoding);
            writer.Write(result.Code);

            var actualDetectionResult = CharsetDetector.DetectFromFile(filePath);
            var actualEncoding = actualDetectionResult.Detected.Encoding;

            encoding.Should().Be(actualEncoding);
        }
    }
}