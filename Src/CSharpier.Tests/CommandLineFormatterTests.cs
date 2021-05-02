using System.Collections.Generic;
using System.CommandLine;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    [TestFixture]
    public class CommandLineFormatterTests
    {
        private MockFileSystem fileSystem;
        private const string UnformattedClass =
            "public class ClassName { public int Field; }";
        private const string FormattedClass =
            "public class ClassName\n{\n    public int Field;\n}\n";

        [SetUp]
        public void Setup()
        {
            this.fileSystem = new MockFileSystem();
        }

        [Test]
        public void Format_Writes_Failed_To_Compile()
        {
            WhenThereExists("c:/test/Invalid.cs", "asdfasfasdf");

            var result = this.Format("c:/test");

            result.lines.First()
                .Should()
                .Be(@"/Invalid.cs - failed to compile");
        }

        [Test]
        public void Format_Writes_File()
        {
            const string unformattedFilePath = "c:/test/Unformatted.cs";
            WhenThereExists(unformattedFilePath, UnformattedClass);

            this.Format("c:/test");

            this.GetFileContent(unformattedFilePath)
                .Should()
                .Be(FormattedClass);
        }

        [Test]
        public void Format_Supports_Skip_Write()
        {
            const string unformattedFilePath = "c:/test/Unformatted.cs";
            WhenThereExists(unformattedFilePath, UnformattedClass);

            this.Format("c:/test", skipWrite: true);

            this.GetFileContent(unformattedFilePath)
                .Should()
                .Be(UnformattedClass);
        }

        [Test]
        public void Format_Checks_Unformatted_File()
        {
            const string unformattedFilePath = "c:/test/Unformatted.cs";
            WhenThereExists(unformattedFilePath, UnformattedClass);

            var (exitCode, lines) = this.Format("c:/test", check: true);

            exitCode.Should().Be(1);
            this.GetFileContent(unformattedFilePath)
                .Should()
                .Be(UnformattedClass);
            lines.First()
                .Should()
                .Contain(@"/Unformatted.cs - was not formatted");
        }

        [Test]
        public void Format_Checks_Formatted_File()
        {
            const string formattedFilePath = "c:/test/Formatted.cs";
            WhenThereExists(formattedFilePath, FormattedClass);

            var (exitCode, lines) = this.Format("c:/test", check: true);

            exitCode.Should().Be(0);
        }

        [TestCase("TemporaryGeneratedFile_Tester.cs")]
        [TestCase("TestFile.designer.cs")]
        [TestCase("TestFile.generated.cs")]
        [TestCase("TestFile.g.cs")]
        [TestCase("TestFile.g.i.cs")]
        public void Format_Skips_Generated_Files(string fileName)
        {
            var unformattedFilePath = $"c:/test/{fileName}";
            WhenThereExists(unformattedFilePath, UnformattedClass);

            var (_, lines) = this.Format("c:/test");

            lines.Should().Contain("Total files: 0 ");
        }

        [TestCase("File.cs", "File.cs")]
        [TestCase("File.cs", "*.cs")]
        [TestCase("SubFolder/File.cs", "*.cs")]
        [TestCase("Debug/Logs/File.cs", "**/Logs")]
        [TestCase("Debug/Logs/File.cs", "Logs/")]
        [TestCase("Debug/Logs/File.cs", "Debug/Logs/File.cs")]
        public void File_In_Ignore_Skips_Formatting(
            string fileName,
            string ignoreContents
        ) {
            var unformattedFilePath = $"c:/test/{fileName}";
            WhenThereExists(unformattedFilePath, UnformattedClass);
            WhenThereExists("c:/test/.csharpierignore", ignoreContents);

            var (_, lines) = this.Format("c:/test");

            lines.FirstOrDefault(o => o.StartsWith("Total files"))
                .Should()
                .Be("Total files: 0 ");
        }

        private (int exitCode, IList<string> lines) Format(
            string rootPath,
            bool skipWrite = false,
            bool check = false
        ) {
            var commandLineFormatter = new TestableCommandLineFormatter(
                rootPath,
                new CommandLineOptions
                {
                    DirectoryOrFile = rootPath,
                    SkipWrite = skipWrite,
                    Check = check
                },
                new PrinterOptions(),
                this.fileSystem
            );

            return (
                commandLineFormatter.Format(CancellationToken.None).Result,
                commandLineFormatter.Lines
            );
        }

        private string GetFileContent(string path)
        {
            return this.fileSystem.File.ReadAllText(path);
        }

        private void WhenThereExists(string path, string contents)
        {
            this.fileSystem.AddFile(path, new MockFileData(contents));
        }

        private class TestableCommandLineFormatter : CommandLineFormatter
        {
            public IList<string> Lines = new List<string>();

            public TestableCommandLineFormatter(
                string rootPath,
                CommandLineOptions commandLineOptions,
                PrinterOptions printerOptions,
                IFileSystem fileSystem
            )
                : base(
                    rootPath,
                    commandLineOptions,
                    printerOptions,
                    fileSystem
                ) { }

            protected override void WriteLine(string? line = null)
            {
                while (line != null && line.Contains("  "))
                {
                    line = line.Replace("  ", " ");
                }

                if (line != null)
                {
                    this.Lines.Add(line.Replace("\\", "/"));
                }
            }
        }
    }
}
