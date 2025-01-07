using CSharpier.Cli.Options;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.Cli.Options;

[TestFixture]
public class ConfigFileParserTests
{
    [Test]
    public void Should_Parse_Yaml_With_Overrides()
    {
        var options = ConfigFileParser.CreateFromContent(
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
        var options = ConfigFileParser.CreateFromContent(
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
}
