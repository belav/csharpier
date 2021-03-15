using System.IO;
using System.Text;
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
                Directory.GetCurrentDirectory());
            while (this.rootDirectory.Name != "CSharpier.Tests")
            {
                this.rootDirectory = this.rootDirectory.Parent;
            }
            EmptyFiles.Extensions.AddTextExtension(".cst");

            DiffTools.AddToolBasedOn(
                DiffTool.WinMerge,
                name: "WritableWinMerge",
                arguments: (temp, target) =>
                {
                    var leftTitle = Path.GetFileName(temp);
                    var rightTitle = Path.GetFileName(target);
                    return $"/u /wr /e \"{temp}\" \"{target}\" /dl \"{leftTitle}\" /dr \"{rightTitle}\"";
                });
        }

        protected void RunTest(string folderName, string fileName)
        {
            var filePath = Path.Combine(
                this.rootDirectory.FullName,
                "TestFiles",
                folderName,
                fileName + ".cst");
            var code = File.ReadAllText(filePath);

            var formatter = new CodeFormatter();
            var result = formatter.Format(code, new Options());

            var actualFilePath = filePath.Replace(".cst", ".actual.cst");
            File.WriteAllText(actualFilePath, result.Code, Encoding.UTF8);

            var targetFilePath = filePath;
            var expectedFilePath = actualFilePath.Replace(
                ".actual.",
                ".expected.");
            if (File.Exists(expectedFilePath))
            {
                code = File.ReadAllText(expectedFilePath, Encoding.UTF8);
                targetFilePath = expectedFilePath;
            }

            if (result.Code != code)
            {
                DiffRunner.Launch(targetFilePath, actualFilePath);
            }
            result.Code.Should().Be(code);
        }
    }
}
