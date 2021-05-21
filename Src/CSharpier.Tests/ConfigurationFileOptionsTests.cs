using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    [TestFixture]
    public class ConfigurationFileOptionsTests
    {
        private MockFileSystem fileSystem;

        [SetUp]
        public void SetUp()
        {
            this.fileSystem = new MockFileSystem();
        }

        [Test]
        public void Should_Return_Default_Options_With_Empty_Json()
        {
            WhenThereExists("c:/test/.csharpierrc", "{}");

            var result = CreateConfigurationOptions("c:/test");

            ShouldHaveDefaultOptions(result);
        }

        [Test]
        public void Should_Return_Default_Options_With_No_File()
        {
            var result = CreateConfigurationOptions("c:/test");

            ShouldHaveDefaultOptions(result);
        }

        [Test]
        public void Should_Return_Json_Extension_Options()
        {
            WhenThereExists("c:/test/.csharpierrc.json", "{ \"printWidth\": 10 }");

            var result = CreateConfigurationOptions("c:/test");

            result.PrintWidth.Should().Be(10);
        }

        [TestCase("yaml")]
        [TestCase("yml")]
        public void Should_Return_Yaml_Extension_Options(string extension)
        {
            WhenThereExists($"c:/test/.csharpierrc.{extension}", "printWidth: 10");

            var result = CreateConfigurationOptions("c:/test");

            result.PrintWidth.Should().Be(10);
        }

        [TestCase("{ \"printWidth\": 10 }")]
        [TestCase("printWidth: 10")]
        public void Should_Read_ExtensionLess_File(string contents)
        {
            WhenThereExists($"c:/test/.csharpierrc", contents);

            var result = CreateConfigurationOptions("c:/test");

            result.PrintWidth.Should().Be(10);
        }

        [TestCase("", "printWidth: 10")]
        [TestCase("", "{ \"printWidth\": 10 }")]
        [TestCase(".yml", "printWidth: 10")]
        [TestCase(".yaml", "printWidth: 10")]
        [TestCase(".json", "{ \"printWidth\": 10 }")]
        public void Should_Find_Configuration_In_Parent_Directory(string extension, string contents)
        {
            WhenThereExists($"c:/test/.csharpierrc{extension}", contents);

            var result = CreateConfigurationOptions("c:/test/subfolder");

            result.PrintWidth.Should().Be(10);
        }

        [Test]
        public void Should_Prefer_No_Extension()
        {
            WhenThereExists("c:/test/.csharpierrc", "{ \"printWidth\": 1 }");

            WhenThereExists("c:/test/.csharpierrc.json", "{ \"printWidth\": 2 }");
            WhenThereExists("c:/test/.csharpierrc.yaml", "printWidth: 3");

            var result = CreateConfigurationOptions("c:/test");

            result.PrintWidth.Should().Be(1);
        }

        [Test]
        public void Should_Return_PrintWidth_With_Json()
        {
            WhenThereExists("c:/test/.csharpierrc", "{ \"printWidth\": 10 }");

            var result = CreateConfigurationOptions("c:/test");

            result.PrintWidth.Should().Be(10);
        }

        [Test]
        public void Should_Return_TabWidth_With_Json()
        {
            WhenThereExists("c:/test/.csharpierrc", "{ \"tabWidth\": 10 }");

            var result = CreateConfigurationOptions("c:/test");

            result.TabWidth.Should().Be(10);
        }

        [Test]
        public void Should_Return_UseTabs_With_Json()
        {
            WhenThereExists("c:/test/.csharpierrc", "{ \"useTabs\": true }");

            var result = CreateConfigurationOptions("c:/test");

            result.UseTabs.Should().BeTrue();
        }

        [Test]
        public void Should_Return_EndOfLine_With_Json()
        {
            WhenThereExists("c:/test/.csharpierrc", "{ \"endOfLine\": \"crlf\" }");

            var result = CreateConfigurationOptions("c:/test");

            result.EndOfLine.Should().Be(EndOfLine.CRLF);
        }

        [Test]
        public void Should_Return_PrintWidth_With_Yaml()
        {
            WhenThereExists("c:/test/.csharpierrc", "printWidth: 10");

            var result = CreateConfigurationOptions("c:/test");

            result.PrintWidth.Should().Be(10);
        }

        [Test]
        public void Should_Return_TabWidth_With_Yaml()
        {
            WhenThereExists("c:/test/.csharpierrc", "tabWidth: 10");

            var result = CreateConfigurationOptions("c:/test");

            result.TabWidth.Should().Be(10);
        }

        [Test]
        public void Should_Return_UseTabs_With_Yaml()
        {
            WhenThereExists("c:/test/.csharpierrc", "useTabs: true");

            var result = CreateConfigurationOptions("c:/test");

            result.UseTabs.Should().BeTrue();
        }

        [Test]
        public void Should_Return_EndOfLine_With_Yaml()
        {
            WhenThereExists("c:/test/.csharpierrc", "endOfLine: crlf");

            var result = CreateConfigurationOptions("c:/test");

            result.EndOfLine.Should().Be(EndOfLine.CRLF);
        }

        private void ShouldHaveDefaultOptions(ConfigurationFileOptions configurationFileOptions)
        {
            configurationFileOptions.PrintWidth.Should().Be(100);
            configurationFileOptions.TabWidth.Should().Be(4);
            configurationFileOptions.UseTabs.Should().BeFalse();
            configurationFileOptions.EndOfLine.Should().Be(EndOfLine.Auto);
        }

        private ConfigurationFileOptions CreateConfigurationOptions(string baseDirectoryPath)
        {
            this.fileSystem.AddDirectory(baseDirectoryPath);
            return ConfigurationFileOptions.Create(baseDirectoryPath, fileSystem);
        }

        private void WhenThereExists(string path, string contents)
        {
            this.fileSystem.AddFile(path, new MockFileData(contents));
        }
    }
}
