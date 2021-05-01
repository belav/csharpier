using System;
using System.IO;
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
            this.rootDirectory = new DirectoryInfo(
                Directory.GetCurrentDirectory()
            );
            while (this.rootDirectory.Name != "CSharpier.Tests")
            {
                this.rootDirectory = this.rootDirectory.Parent;
            }
        }

        protected void RunTest(string folderName, string fileName)
        {
            var filePath = Path.Combine(
                this.rootDirectory.FullName,
                "TestFiles",
                folderName,
                fileName + ".cst"
            );
            var fileReaderResult =
                FileReader.ReadFile(filePath, CancellationToken.None).Result;

            var formatter = new CodeFormatter();
            var result = formatter.Format(
                fileReaderResult.FileContents,
                new Options()
                {
                    Width = Options.TestingWidth,
                    EndOfLine = Environment.NewLine
                }
            );

            var actualFilePath = filePath.Replace(".cst", ".actual.cst");
            File.WriteAllText(
                actualFilePath,
                result.Code,
                fileReaderResult.Encoding
            );

            var filePathToChange = filePath;
            var expectedFilePath = actualFilePath.Replace(
                ".actual.",
                ".expected."
            );

            var expectedCode = fileReaderResult.FileContents;

            if (File.Exists(expectedFilePath))
            {
                expectedCode = File.ReadAllText(
                    expectedFilePath,
                    Encoding.UTF8
                );
                filePathToChange = expectedFilePath;
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
