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
    [Arguments("true", true)]
    [Arguments("True", true)]
    [Arguments("TRUE", true)]
    [Arguments("yes", true)]
    [Arguments("Yes", true)]
    [Arguments("y", true)]
    [Arguments("on", true)]
    [Arguments("ON", true)]
    [Arguments("\"true\"", true)]
    [Arguments("\"yes\"", true)]
    [Arguments("false", false)]
    [Arguments("no", false)]
    [Arguments("n", false)]
    [Arguments("off", false)]
    [Arguments("~", false)]
    [Arguments("null", false)]
    [Arguments("", false)]
    public void Should_Parse_Yaml_Boolean_Spellings(string value, bool expected)
    {
        var options = CSharpierConfigParser.CreateFromContent(
            $"printWidth: 80\nuseTabs: {value}\noverrides:\n  - files: \"*.cst\"\n    useTabs: {value}"
        );

        options.UseTabs.Should().Be(expected);
        options.Overrides.Should().ContainSingle().Which.UseTabs.Should().Be(expected);
    }

    [Test]
    [Arguments("80", 80)]
    [Arguments("\"80\"", 80)]
    [Arguments("+5", 5)]
    [Arguments("-5", -5)]
    [Arguments("1_000", 1000)]
    [Arguments("0x10", 16)]
    [Arguments("010", 8)]
    public void Should_Parse_Yaml_Number_Formats(string value, int expected)
    {
        var options = CSharpierConfigParser.CreateFromContent($"printWidth: {value}");

        options.PrintWidth.Should().Be(expected);
    }

    [Test]
    [Arguments("~")]
    [Arguments("null")]
    [Arguments("")]
    public void Should_Treat_Null_Yaml_Values_As_Not_Set(string value)
    {
        var options = CSharpierConfigParser.CreateFromContent(
            $"useTabs: true\nprintWidth: {value}\nendOfLine: {value}\nxmlWhitespaceSensitivity: {value}\noverrides: {value}"
        );

        options.UseTabs.Should().BeTrue();
        options.PrintWidth.Should().BeNull();
        options.EndOfLine.Should().Be(EndOfLine.Auto);
        options.XmlWhitespaceSensitivity.Should().BeNull();
        options.Overrides.Should().BeEmpty();
    }

    [Test]
    public void Should_Parse_Yaml_Enum_From_Number()
    {
        var options = CSharpierConfigParser.CreateFromContent("endOfLine: 1");

        options.EndOfLine.Should().Be(EndOfLine.CRLF);
    }

    [Test]
    public void Should_Match_Yaml_Options_With_Exact_Casing_Only()
    {
        var options = CSharpierConfigParser.CreateFromContent("PrintWidth: 5\nusetabs: true");

        options.PrintWidth.Should().BeNull();
        options.UseTabs.Should().BeFalse();
    }

    [Test]
    public void Should_Return_Defaults_For_Yaml_With_Only_Comments()
    {
        var options = CSharpierConfigParser.CreateFromContent("# only a comment");

        options.PrintWidth.Should().BeNull();
        options.Overrides.Should().BeEmpty();
    }

    [Test]
    [Arguments("useTabs: 1")]
    [Arguments("useTabs: maybe")]
    [Arguments("printWidth: 0o10")]
    [Arguments("printWidth: 80.0")]
    [Arguments("printWidth: [1]")]
    [Arguments("endOfLine: nope")]
    [Arguments("overrides: [oops]")]
    [Arguments("overrides: oops")]
    [Arguments("overrides:\n  files: \"*.cs\"")]
    [Arguments("overrides:\n  - ~")]
    [Arguments("overrides:\n  - files: [a, b]")]
    [Arguments("overrides:\n  - files: \"*.cs\"\n    useTabs: maybe")]
    [Arguments("oops")]
    [Arguments("- a\n- b")]
    public void Should_Throw_For_Invalid_Yaml(string content)
    {
        var action = () => CSharpierConfigParser.CreateFromContent(content);

        action.Should().Throw<YamlDotNet.Core.YamlException>();
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
