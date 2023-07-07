namespace CSharpier.Tests;

using System.IO.Abstractions.TestingHelpers;
using CSharpier.Cli.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class OptionsProviderTests
{
    [Test]
    public async Task Should_Return_Default_Options_With_Empty_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{}");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        ShouldHaveDefaultOptions(result);
    }

    [Test]
    public async Task Should_Return_Default_Options_With_No_File()
    {
        var context = new TestContext();
        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        ShouldHaveDefaultOptions(result);
    }

    [TestCase(".csharpierrc")]
    [TestCase(".csharpierrc.json")]
    [TestCase(".csharpierrc.yaml")]
    public async Task Should_Return_Default_Options_With_Empty_File(string fileName)
    {
        var context = new TestContext();
        context.WhenAFileExists($"c:/test/{fileName}", string.Empty);
        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        ShouldHaveDefaultOptions(result);
    }

    [Test]
    public async Task Should_Return_Json_Extension_Options()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.csharpierrc.json",
            @"{ 
    ""printWidth"": 10, 
    ""preprocessorSymbolSets"": [""1,2"", ""3""]
}"
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(10);
    }

    [TestCase("yaml")]
    [TestCase("yml")]
    public async Task Should_Return_Yaml_Extension_Options(string extension)
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

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(10);
    }

    [TestCase("{ \"printWidth\": 10 }")]
    [TestCase("printWidth: 10")]
    public async Task Should_Read_ExtensionLess_File(string contents)
    {
        var context = new TestContext();
        context.WhenAFileExists($"c:/test/.csharpierrc", contents);

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(10);
    }

    [TestCase("", "printWidth: 10")]
    [TestCase("", "{ \"printWidth\": 10 }")]
    [TestCase(".yml", "printWidth: 10")]
    [TestCase(".yaml", "printWidth: 10")]
    [TestCase(".json", "{ \"printWidth\": 10 }")]
    public async Task Should_Find_Configuration_In_Parent_Directory(
        string extension,
        string contents
    )
    {
        var context = new TestContext();
        context.WhenAFileExists($"c:/test/.csharpierrc{extension}", contents);

        var optionsProvider = await context.CreateOptionsProvider("c:/test/subfolder");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Prefer_No_Extension()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{ \"printWidth\": 1 }");

        context.WhenAFileExists("c:/test/.csharpierrc.json", "{ \"printWidth\": 2 }");
        context.WhenAFileExists("c:/test/.csharpierrc.yaml", "printWidth: 3");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(1);
    }

    [Test]
    public async Task Should_Return_PrintWidth_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{ \"printWidth\": 10 }");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_TabWidth_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{ \"tabWidth\": 10 }");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.TabWidth.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_UseTabs_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "{ \"useTabs\": true }");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.UseTabs.Should().BeTrue();
    }

    [Test]
    public async Task Should_Return_PrintWidth_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "printWidth: 10");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_TabWidth_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "tabWidth: 10");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.TabWidth.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_UseTabs_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("c:/test/.csharpierrc", "useTabs: true");

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.UseTabs.Should().BeTrue();
    }

    [Test]
    public async Task Should_Support_EditorConfig_Basic()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.editorConfig",
            @"
[*]
indent_style = space
indent_size = 2
max_line_length = 10
"
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.UseTabs.Should().BeFalse();
        result.TabWidth.Should().Be(2);
        result.Width.Should().Be(10);
    }

    [TestCase("tab_width")]
    [TestCase("indent_size")]
    public async Task Should_Support_EditorConfig_Tabs(string propertyName)
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.editorConfig",
            $@"
    [*]
    indent_style = tab
    {propertyName} = 2
    "
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.UseTabs.Should().BeTrue();
        result.TabWidth.Should().Be(2);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Tab_Width()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.editorConfig",
            @"
    [*]
    indent_style = tab
    indent_size = 1
    tab_width = 3
    "
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.UseTabs.Should().BeTrue();
        result.TabWidth.Should().Be(3);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Indent_Size_Tab()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.editorConfig",
            @"
    [*]
    indent_size = tab
    tab_width = 3
    "
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");

        result.TabWidth.Should().Be(3);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Multiple_Files()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/sub1/.editorConfig",
            @"
    [*]
    indent_size = 1
    "
        );

        context.WhenAFileExists(
            "c:/test/.editorConfig",
            @"
    [*]
    indent_size = 2
    max_line_length = 10
    "
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test/sub1");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");
        result.TabWidth.Should().Be(1);
        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Multiple_Files_And_Unset()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/sub1/.editorConfig",
            @"
    [*]
    indent_size = unset
    "
        );

        context.WhenAFileExists(
            "c:/test/.editorConfig",
            @"
    [*]
    indent_size = 2
    "
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test/sub1");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");
        result.TabWidth.Should().Be(4);
    }

    // TODO 1 fix this
    // TODO 1 write some tests that check for csharpier vs editorconfig
    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Globs()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "c:/test/.editorConfig",
            @"
[*]
indent_size = 1

[*.cs]
indent_size = 2
"
        );

        var optionsProvider = await context.CreateOptionsProvider("c:/test");
        var result = optionsProvider.GetPrinterOptionsFor("c:/test/test.cs");
        result.TabWidth.Should().Be(2);
    }

    private static void ShouldHaveDefaultOptions(PrinterOptions printerOptions)
    {
        printerOptions.Width.Should().Be(100);
        printerOptions.TabWidth.Should().Be(4);
        printerOptions.UseTabs.Should().BeFalse();
    }

    private class TestContext
    {
        private readonly MockFileSystem fileSystem = new();

        public void WhenAFileExists(string path, string contents)
        {
            this.fileSystem.AddFile(path, new MockFileData(contents));
        }

        public Task<OptionsProvider> CreateOptionsProvider(string directoryName)
        {
            this.fileSystem.AddDirectory(directoryName);
            return OptionsProvider.Create(
                directoryName,
                null,
                this.fileSystem,
                NullLogger.Instance,
                CancellationToken.None
            );
        }
    }
}
