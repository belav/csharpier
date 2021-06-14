using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading;
using DiffEngine;
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

        protected void RunTest(string folderName, string fileName, bool useTabs = false)
        {
            var filePath = Path.Combine(
                this.rootDirectory.FullName,
                "TestFiles",
                folderName,
                fileName + ".cst"
            );
            var fileReaderResult = FileReader.ReadFile(
                    filePath,
                    new FileSystem(),
                    CancellationToken.None
                ).Result;

            var formatter = new CodeFormatter();
            var result = formatter.Format(
                fileReaderResult.FileContents,
                new PrinterOptions() { Width = PrinterOptions.WidthUsedByTests, UseTabs = useTabs }
            );

            var actualFilePath = filePath.Replace(".cst", ".actual.cst");
            File.WriteAllText(actualFilePath, result.Code, fileReaderResult.Encoding);

            var filePathToChange = filePath;
            var expectedFilePath = actualFilePath.Replace(".actual.", ".expected.");

            var expectedCode = fileReaderResult.FileContents;

            if (File.Exists(expectedFilePath))
            {
                expectedCode = File.ReadAllText(expectedFilePath, Encoding.UTF8);
                filePathToChange = expectedFilePath;
            }

            if (Environment.GetEnvironmentVariable("NormalizeLineEndings") != null)
            {
                expectedCode = expectedCode.Replace("\r\n", "\n");
                result.Code = result.Code.Replace("\r\n", "\n");
            }

            var comparer = new SyntaxNodeComparer(
                expectedCode,
                result.Code,
                CancellationToken.None
            );

            result.Errors.Should().BeEmpty();
            result.FailureMessage.Should().BeEmpty();

            if (result.Code != expectedCode && !BuildServerDetector.Detected)
            {
                DiffRunner.Launch(filePathToChange, actualFilePath);
            }
            result.Code.Should().Be(expectedCode);

            var compareResult = comparer.CompareSource();
            compareResult.Should().BeNullOrEmpty();
        }
    }
}
