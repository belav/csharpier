using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using CSharpier.Cli;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
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
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{}");

        var result = context.CreateConfigurationOptions("c:/test");

        ShouldHaveDefaultOptions(result);
    }

    [Test]
    public void Should_Return_Default_Options_With_No_File()
    {
        var context = new TestContext();
        var result = context.CreateConfigurationOptions("c:/test");

        ShouldHaveDefaultOptions(result);
    }

    [TestCase(".csharpierrc")]
    [TestCase(".csharpierrc.json")]
    [TestCase(".csharpierrc.yaml")]
    public void Should_Return_Default_Options_With_Empty_File(string fileName)
    {
        var context = new TestContext();
        context.WhenAFileExists($"c:/test/{fileName}", string.Empty);
        var result = context.CreateConfigurationOptions("c:/test");

        ShouldHaveDefaultOptions(result);
    }

    [Test]
    public void Should_Return_Json_Extension_Options()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.csharpierrc.json",
            @"{ 
    ""printWidth"": 10, 
    ""preprocessorSymbolSets"": [""1,2"", ""3""]
}"
        );

        var result = context.CreateConfigurationOptions("c:/test");

        result.PrintWidth.Should().Be(10);
        result.PreprocessorSymbolSets.Should().BeEquivalentTo(new List<string> { "1,2", "3" });
    }

    [TestCase("yaml")]
    [TestCase("yml")]
    public void Should_Return_Yaml_Extension_Options(string extension)
    {
        var context = new TestContext();
        context.WhenAFileExists(
            $"c:/test/.csharpierrc.{extension}",
            @"
printWidth: 10
preprocessorSymbolSets: 
  - 1,2
  - 3
"
        );

        var result = context.CreateConfigurationOptions("c:/test");

        result.PrintWidth.Should().Be(10);
        result.PreprocessorSymbolSets.Should().BeEquivalentTo(new List<string> { "1,2", "3" });
    }

    [TestCase("{ \"printWidth\": 10 }")]
    [TestCase("printWidth: 10")]
    public void Should_Read_ExtensionLess_File(string contents)
    {
        var context = new TestContext();
        context.WhenAFileExists($"c:/test/.csharpierrc", contents);

        var result = context.CreateConfigurationOptions("c:/test");

        result.PrintWidth.Should().Be(10);
    }

    [TestCase("", "printWidth: 10")]
    [TestCase("", "{ \"printWidth\": 10 }")]
    [TestCase(".yml", "printWidth: 10")]
    [TestCase(".yaml", "printWidth: 10")]
    [TestCase(".json", "{ \"printWidth\": 10 }")]
    public void Should_Find_Configuration_In_Parent_Directory(string extension, string contents)
    {
        var context = new TestContext();
        context.WhenAFileExists($"c:/test/.csharpierrc{extension}", contents);

        var result = context.CreateConfigurationOptions("c:/test/subfolder");

        result.PrintWidth.Should().Be(10);
    }

    [Test]
    public void Should_Prefer_No_Extension()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{ \"printWidth\": 1 }");

        context.WhenAFileExists("c:/test/.csharpierrc.json", "{ \"printWidth\": 2 }");
        context.WhenAFileExists("c:/test/.csharpierrc.yaml", "printWidth: 3");

        var result = context.CreateConfigurationOptions("c:/test");

        result.PrintWidth.Should().Be(1);
    }

    [Test]
    public void Should_Return_PrintWidth_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{ \"printWidth\": 10 }");

        var result = context.CreateConfigurationOptions("c:/test");

        result.PrintWidth.Should().Be(10);
    }

    [Test]
    public void Should_Return_PrintWidth_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "printWidth: 10");

        var result = context.CreateConfigurationOptions("c:/test");

        result.PrintWidth.Should().Be(10);
    }

    private static void ShouldHaveDefaultOptions(ConfigurationFileOptions configurationFileOptions)
    {
        configurationFileOptions.PrintWidth.Should().Be(100);
        configurationFileOptions.PreprocessorSymbolSets.Should().BeNull();
    }

    private class TestContext
    {
        private MockFileSystem fileSystem = new();

        public ConfigurationFileOptions CreateConfigurationOptions(string baseDirectoryPath)
        {
            this.fileSystem.AddDirectory(baseDirectoryPath);
            return ConfigurationFileOptions.Create(baseDirectoryPath, this.fileSystem);
        }

        public void WhenAFileExists(string path, string contents)
        {
            this.fileSystem.AddFile(path, new MockFileData(contents));
        }
    }
}
