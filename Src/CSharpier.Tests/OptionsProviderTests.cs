using System.IO.Abstractions.TestingHelpers;
using AwesomeAssertions;
using CSharpier.Cli.Options;
using CSharpier.Core;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpier.Tests;

public class OptionsProviderTests
{
    [Test]
    public async Task Should_Return_Default_CSharp_Options_With_Empty_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "{}");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        ShouldHaveDefaultCSharpOptions(result);
    }

    [Test]
    [Arguments("xml")]
    [Arguments("xaml")]
    public async Task Should_Return_Default_Xml_Options_With_Empty_Json(string extension)
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "{}");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test." + extension);

        ShouldHaveDefaultXmlOptions(result, extension);
    }

    [Test]
    [Arguments("cs")]
    [Arguments("csx")]
    public async Task Should_Return_Default_Options_With_No_Config_File_And_Known_CSharp_Extension(
        string extension
    )
    {
        var context = new TestContext();
        var result = await context.CreateProviderAndGetOptionsFor(".", "./test." + extension);

        ShouldHaveDefaultCSharpOptions(result);
    }

    [Test]
    [Arguments("config")]
    [Arguments("csproj")]
    [Arguments("props")]
    [Arguments("slnx")]
    [Arguments("targets")]
    [Arguments("xaml")]
    [Arguments("axaml")]
    [Arguments("xml")]
    public async Task Should_Return_Default_Options_With_No_File_And_Known_Xml_Extension(
        string extension
    )
    {
        var context = new TestContext();
        var result = await context.CreateProviderAndGetOptionsFor(".", "./test." + extension);

        ShouldHaveDefaultXmlOptions(result, extension);
    }

    [Test]
    public async Task Should_Throw_Exception_With_No_Config_File_And_Unknown_Extension()
    {
        var context = new TestContext();
        var result = async () => await context.CreateProviderAndGetOptionsFor(".", "./test.bad");

        await result.Should().ThrowAsync<Exception>();
    }

    [Test]
    [Arguments(".csharpierrc")]
    [Arguments(".csharpierrc.json")]
    [Arguments(".csharpierrc.yaml")]
    public async Task Should_Return_Default_Options_With_Empty_File(string fileName)
    {
        var context = new TestContext();
        context.WhenAFileExists($"./{fileName}", string.Empty);
        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        ShouldHaveDefaultCSharpOptions(result);
    }

    [Test]
    public async Task Should_Return_Json_Extension_Options()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.csharpierrc.json",
            """
{ 
    "printWidth": 10, 
    "endOfLine": "crlf",
    "xmlWhitespaceSensitivity": "ignore"
}
"""
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.Width.Should().Be(10);
        result.EndOfLine.Should().Be(EndOfLine.CRLF);
        result.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
    }

    [Test]
    [Arguments("yaml")]
    [Arguments("yml")]
    public async Task Should_Return_Yaml_Extension_Options(string extension)
    {
        var context = new TestContext();
        context.WhenAFileExists(
            $"./.csharpierrc.{extension}",
            """
printWidth: 10
endOfLine: crlf
xmlWhitespaceSensitivity: ignore
"""
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.Width.Should().Be(10);
        result.EndOfLine.Should().Be(EndOfLine.CRLF);
        result.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
    }

    [Test]
    [Arguments("{ \"printWidth\": 10 }")]
    [Arguments("printWidth: 10")]
    public async Task Should_Read_ExtensionLess_File(string contents)
    {
        var context = new TestContext();
        context.WhenAFileExists($"./.csharpierrc", contents);

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.Width.Should().Be(10);
    }

    [Test]
    [Arguments("", "printWidth: 10")]
    [Arguments("", "{ \"printWidth\": 10 }")]
    [Arguments(".yml", "printWidth: 10")]
    [Arguments(".yaml", "printWidth: 10")]
    [Arguments(".json", "{ \"printWidth\": 10 }")]
    public async Task Should_Find_Configuration_In_Parent_Directory(
        string extension,
        string contents
    )
    {
        var context = new TestContext();
        context.WhenAFileExists($"./.csharpierrc{extension}", contents);

        var result = await context.CreateProviderAndGetOptionsFor(
            "./subfolder",
            "./subfolder/test.cs"
        );

        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Prefer_No_Extension()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "{ \"printWidth\": 1 }");

        context.WhenAFileExists("./.csharpierrc.json", "{ \"printWidth\": 2 }");
        context.WhenAFileExists("./.csharpierrc.yaml", "printWidth: 3");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.Width.Should().Be(1);
    }

    [Test]
    public async Task Should_Return_PrintWidth_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "{ \"printWidth\": 10 }");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_IndentSize_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "{ \"indentSize\": 10 }");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_UseTabs_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "{ \"useTabs\": true }");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.UseTabs.Should().BeTrue();
    }

    [Test]
    public async Task Should_Return_PrintWidth_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "printWidth: 10");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_IndentSize_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "indentSize: 10");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(10);
    }

    [Test]
    public async Task Should_Return_UseTabs_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists("./.csharpierrc", "useTabs: true");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.UseTabs.Should().BeTrue();
    }

    [Test]
    public async Task Should_Return_IndentSize_For_Override_With_Yaml()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.csharpierrc",
            """
overrides:
    - files: "*.{override,another}"
      formatter: "csharp"
      indentSize: 2
      xmlWhitespaceSensitivity: ignore
"""
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.override");

        result.IndentSize.Should().Be(2);
        result.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
    }

    [Test]
    public async Task Should_Return_IndentSize_For_Override_With_Json()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.csharpierrc",
            """
{
    "overrides": [
        {
            "files": "*.{override,another}",
            "formatter": "csharp",
            "indentSize": 2,
            "xmlWhitespaceSensitivity": "ignore"
        }
    ]
}
"""
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.override");

        result.IndentSize.Should().Be(2);
        result.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
    }

    [Test]
    [Arguments("xml", 2)]
    [Arguments("csharp", 4)]
    public async Task Should_Return_Formatter_Default_IndentSize_For_Override_Without_IndentSize(
        string formatter,
        int expectedIndentSize
    )
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.csharpierrc",
            $"""
overrides:
    - files: "*.override"
      formatter: "{formatter}"
"""
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.override");

        result.IndentSize.Should().Be(expectedIndentSize);
    }

    [Test]
    [Arguments("cs")]
    [Arguments("csx")]
    public async Task Should_Return_Default_CSharp_Options_With_Empty_EditorConfig(string extension)
    {
        var context = new TestContext();
        context.WhenAFileExists("./.editorconfig", string.Empty);

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test." + extension);
        ShouldHaveDefaultCSharpOptions(result);
    }

    [Test]
    [Arguments("xml")]
    [Arguments("xaml")]
    [Arguments("csproj")]
    [Arguments("props")]
    [Arguments("targets")]
    [Arguments("config")]
    public async Task Should_Return_Default_Xml_Options_With_Empty_EditorConfig(string extension)
    {
        var context = new TestContext();
        context.WhenAFileExists("./.editorconfig", string.Empty);

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test." + extension);
        ShouldHaveDefaultXmlOptions(result, extension);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Basic()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_style = space
            indent_size = 2
            max_line_length = 10
            end_of_line = crlf
            csharpier_xml_whitespace_sensitivity = ignore
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.UseTabs.Should().BeFalse();
        result.IndentSize.Should().Be(2);
        result.Width.Should().Be(10);
        result.EndOfLine.Should().Be(EndOfLine.CRLF);
        result.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
    }

    [Test]
    public async Task Should_Support_EditorConfig_With_Comments()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            # EditorConfig is awesome: https://EditorConfig.org

            # top-most EditorConfig file
            root = true

            [*]
            indent_style = space
            indent_size = 2
            max_line_length = 10
            ; Windows-style line endings
            end_of_line = crlf

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.UseTabs.Should().BeFalse();
        result.IndentSize.Should().Be(2);
        result.Width.Should().Be(10);
        result.EndOfLine.Should().Be(EndOfLine.CRLF);
    }

    [Test]
    public async Task Should_Support_EditorConfig_With_Duplicated_Sections()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_size = 2

            [*]
            indent_size = 4

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(4);
    }

    [Test]
    public async Task Should_Support_EditorConfig_With_Duplicated_Rules()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_size = 2
            indent_size = 4

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(4);
    }

    [Test]
    public async Task Should_Not_Fail_With_Bad_EditorConfig()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*
            indent_size==

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(4);
    }

    [Test]
    [Arguments("tab_width")]
    [Arguments("indent_size")]
    public async Task Should_Support_EditorConfig_Tabs(string propertyName)
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            $"""

                [*]
                indent_style = tab
                {propertyName} = 2
                
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.UseTabs.Should().BeTrue();
        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Tab_Width()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

                [*]
                indent_style = tab
                indent_size = 1
                tab_width = 3
                
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.UseTabs.Should().BeTrue();
        result.IndentSize.Should().Be(3);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Indent_Size_Tab()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

                [*]
                indent_size = tab
                tab_width = 3
                
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(3);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Multiple_Files()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./subfolder/.editorconfig",
            """

                [*]
                indent_size = 1
                
            """
        );

        context.WhenAFileExists(
            "./.editorconfig",
            """

                [*]
                indent_size = 2
                max_line_length = 10
                
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(
            "./subfolder",
            "./subfolder/test.cs"
        );
        result.IndentSize.Should().Be(1);
        result.Width.Should().Be(10);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Multiple_Files_And_Unset()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./subfolder/.editorconfig",
            """

                [*]
                indent_size = unset
                
            """
        );

        context.WhenAFileExists(
            "./.editorconfig",
            """

                [*]
                indent_size = 2
                
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(
            "./subfolder",
            "./subfolder/test.cs"
        );
        result.IndentSize.Should().Be(4);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Root()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./subfolder/.editorconfig",
            """

                root = true

                [*]
                indent_size = 2
                
            """
        );

        context.WhenAFileExists(
            "./.editorconfig",
            """

                [*]
                max_line_length = 2
                
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(
            "./subfolder",
            "./subfolder/test.cs"
        );
        result.Width.Should().Be(100);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Globs()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_size = 1

            [*.cs]
            indent_size = 2

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");
        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Glob_Braces()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_size = 1

            [*.{cs}]
            indent_size = 2

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");
        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Support_EditorConfig_Tabs_With_Glob_Braces_Multiples()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_size = 1

            [*.{csx,cs}]
            indent_size = 2

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");
        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Return_Overrides_In_Editorconfig()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """
            [*.cst]
            indent_size = 2
            csharpier_formatter = csharp
            csharpier_xml_whitespace_sensitivity = ignore
            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cst");

        result.IndentSize.Should().Be(2);
        result.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
    }

    [Test]
    public async Task Should_Find_EditorConfig_In_Parent_Directory()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*.cs]
            indent_size = 2

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(
            "./subfolder",
            "./subfolder/test.cs"
        );
        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Prefer_CSharpierrc_In_SameFolder()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*.cs]
            indent_size = 2

            """
        );
        context.WhenAFileExists("./.csharpierrc", "indentSize: 1");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");
        result.IndentSize.Should().Be(1);
    }

    [Test]
    public async Task Should_Not_Prefer_Closer_EditorConfig()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./subfolder/.editorconfig",
            """

            [*.cs]
            indent_size = 2

            """
        );
        context.WhenAFileExists("./.csharpierrc", "indentSize: 1");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./subfolder/test.cs");
        result.IndentSize.Should().Be(1);
    }

    [Test]
    public async Task Should_Ignore_Invalid_EditorConfig_Lines()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*]
            indent_size = 2
            INVALID

            """
        );

        var result = await context.CreateProviderAndGetOptionsFor(".", "./test.cs");

        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Not_Ignore_Ignored_EditorConfig()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./subfolder/.editorconfig",
            """

                [*]
                indent_size = 2
                
            """
        );

        context.WhenAFileExists(
            "./.editorconfig",
            """

                [*]
                indent_size = 1
                
            """
        );

        context.WhenAFileExists("./.csharpierignore", "/subfolder/.editorconfig");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./subfolder/test.cs");
        result.IndentSize.Should().Be(2);
    }

    [Test]
    public async Task Should_Prefer_Closer_CSharpierrc()
    {
        var context = new TestContext();
        context.WhenAFileExists(
            "./.editorconfig",
            """

            [*.cs]
            indent_size = 2

            """
        );
        context.WhenAFileExists("./subfolder/.csharpierrc", "indentSize: 1");

        var result = await context.CreateProviderAndGetOptionsFor(".", "./subfolder/test.cs");
        result.IndentSize.Should().Be(1);
    }

    [Test]
    [Arguments("xml", XmlWhitespaceSensitivity.Strict)]
    [Arguments("xaml", XmlWhitespaceSensitivity.Ignore)]
    [Arguments("axaml", XmlWhitespaceSensitivity.Ignore)]
    public async Task Should_Default_XmlWhitespaceSensitivity(
        string fileExtension,
        XmlWhitespaceSensitivity expectedXmlWhitespaceSensitivity
    )
    {
        var context = new TestContext();

        var result = await context.CreateProviderAndGetOptionsFor(".", "./file." + fileExtension);

        result.XmlWhitespaceSensitivity.Should().Be(expectedXmlWhitespaceSensitivity);
    }

    private static void ShouldHaveDefaultCSharpOptions(PrinterOptions printerOptions)
    {
        printerOptions.Width.Should().Be(100);
        printerOptions.IndentSize.Should().Be(4);
        printerOptions.UseTabs.Should().BeFalse();
        printerOptions.EndOfLine.Should().Be(EndOfLine.Auto);
    }

    private static void ShouldHaveDefaultXmlOptions(PrinterOptions printerOptions, string extension)
    {
        printerOptions.Width.Should().Be(100);
        printerOptions.IndentSize.Should().Be(2);
        printerOptions.UseTabs.Should().BeFalse();
        printerOptions.EndOfLine.Should().Be(EndOfLine.Auto);
        printerOptions
            .XmlWhitespaceSensitivity.Should()
            .Be(
                extension is "xaml" or "axaml"
                    ? XmlWhitespaceSensitivity.Ignore
                    : XmlWhitespaceSensitivity.Strict
            );
    }

    private sealed class TestContext
    {
        private readonly MockFileSystem fileSystem = new();

        private readonly string uniqueRootPath = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString()
        );

        public void WhenAFileExists(string path, string contents)
        {
            this.fileSystem.AddFile(this.ResolvePath(path), new MockFileData(contents));
        }

        public async Task<PrinterOptions> CreateProviderAndGetOptionsFor(
            string directoryName,
            string filePath
        )
        {
            directoryName = this.ResolvePath(directoryName);
            filePath = this.ResolvePath(filePath);

            this.fileSystem.AddDirectory(directoryName);
            var provider = await OptionsProvider.Create(
                directoryName,
                null,
                null,
                this.fileSystem,
                NullLogger.Instance,
                CancellationToken.None
            );

            var printerOptions =
                await provider.GetPrinterOptionsForAsync(filePath, CancellationToken.None)
                ?? throw new Exception("PrinterOptions was null");

            return printerOptions;
        }

        private string ResolvePath(string path)
        {
            return (this.uniqueRootPath + path[1..]).Replace(
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar
            );
        }
    }
}
