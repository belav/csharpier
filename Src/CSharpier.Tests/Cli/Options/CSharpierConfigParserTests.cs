using AwesomeAssertions;
using CSharpier.Cli.Options;
using CSharpier.Core;

namespace CSharpier.Tests.Cli.Options;

public class CSharpierConfigParserTests
{
    [Test]
    public void Should_Parse_Yaml_With_Overrides()
    {
        var options = CSharpierConfigParser.CreateFromContent(
            """
            overrides:
                - files: "*.cst"
                  formatter: "csharp"
                  indentSize: 2
                  useTabs: true
                  printWidth: 10
                  endOfLine: "LF"
            """
        );

        options.Overrides.Should().HaveCount(1);
        options.Overrides.First().Files.Should().Be("*.cst");
        options.Overrides.First().Formatter.Should().Be("csharp");
        options.Overrides.First().IndentSize.Should().Be(2);
        options.Overrides.First().UseTabs.Should().Be(true);
        options.Overrides.First().PrintWidth.Should().Be(10);
        options.Overrides.First().EndOfLine.Should().Be(EndOfLine.LF);
    }

    [Test]
    public void Should_Parse_Json_With_Overrides()
    {
        var options = CSharpierConfigParser.CreateFromContent(
            """
            {
                "overrides": [
                    {
                       "files": "*.cst",
                       "formatter": "csharp",
                       "indentSize": 2,
                       "useTabs": true,
                       "printWidth": 10,
                       "endOfLine": "LF"
                    }
                ]
            }
            """
        );

        options.Overrides.Should().HaveCount(1);
        options.Overrides.First().Files.Should().Be("*.cst");
        options.Overrides.First().Formatter.Should().Be("csharp");
        options.Overrides.First().IndentSize.Should().Be(2);
        options.Overrides.First().UseTabs.Should().Be(true);
        options.Overrides.First().PrintWidth.Should().Be(10);
        options.Overrides.First().EndOfLine.Should().Be(EndOfLine.LF);
    }

    [Test]
    public void Should_Parse_Yaml_Top_Level_Options()
    {
        var options = CSharpierConfigParser.CreateFromContent(
            """
            # a comment
            printWidth: 80
            indentSize: 2
            useTabs: true
            endOfLine: crlf
            xmlWhitespaceSensitivity: "Ignore"
            """
        );

        options.PrintWidth.Should().Be(80);
        options.IndentSize.Should().Be(2);
        options.UseTabs.Should().BeTrue();
        options.EndOfLine.Should().Be(EndOfLine.CRLF);
        options.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
        options.Overrides.Should().BeEmpty();
    }

    [Test]
    public void Should_Use_Defaults_For_Yaml_Options_That_Are_Not_Set()
    {
        var options = CSharpierConfigParser.CreateFromContent("printWidth: 80");

        options.PrintWidth.Should().Be(80);
        options.IndentSize.Should().BeNull();
        options.UseTabs.Should().BeFalse();
        options.EndOfLine.Should().Be(EndOfLine.Auto);
        options.XmlWhitespaceSensitivity.Should().BeNull();
        options.Overrides.Should().BeEmpty();
    }

    [Test]
    public void Should_Ignore_Unknown_Yaml_Options()
    {
        var options = CSharpierConfigParser.CreateFromContent(
            """
            printWidth: 80
            notAnOption: true
            nested:
              thing: 1
              list:
                - a
            overrides:
                - files: "*.cst"
                  formatter: "csharp"
                  alsoUnknown: [1, 2]
            """
        );

        options.PrintWidth.Should().Be(80);
        options.Overrides.Should().HaveCount(1);
        options.Overrides.First().Files.Should().Be("*.cst");
    }

    [Test]
    public void Should_Parse_Yaml_Override_Xml_Options()
    {
        var options = CSharpierConfigParser.CreateFromContent(
            """
            overrides:
                - files: "*.config"
                  formatter: "xml"
                  xmlWhitespaceSensitivity: "Strict"
                - files: "*.other"
                  formatter: "xml"
            """
        );

        options.Overrides.Should().HaveCount(2);
        options.Overrides[0].XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Strict);
        options.Overrides[1].XmlWhitespaceSensitivity.Should().BeNull();
        options.Overrides[1].Files.Should().Be("*.other");
    }

    [Test]
    public void Should_Throw_For_Yaml_Option_With_Invalid_Value()
    {
        var action = () => CSharpierConfigParser.CreateFromContent("printWidth: wide");

        action.Should().Throw<Exception>();
    }

    [Test]
    public void Should_Parse_Json_With_Any_Property_Casing()
    {
        var options = CSharpierConfigParser.CreateFromContent(
            """
            {
                "PRINTWIDTH": 80,
                "IndentSize": 2,
                "usetabs": true,
                "endOfLine": "crlf",
                "xmlWhitespaceSensitivity": "ignore"
            }
            """
        );

        options.PrintWidth.Should().Be(80);
        options.IndentSize.Should().Be(2);
        options.UseTabs.Should().BeTrue();
        options.EndOfLine.Should().Be(EndOfLine.CRLF);
        options.XmlWhitespaceSensitivity.Should().Be(XmlWhitespaceSensitivity.Ignore);
        options.Overrides.Should().BeEmpty();
    }
}
