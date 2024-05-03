namespace CSharpier.Tests.Cli.Options;

using CSharpier.Cli.Options;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class ConfigurationFileParserTests
{
    [Test]
    public void Should_Parse_Yaml_With_Overrides()
    {
        var options = ConfigurationFileParser.CreateFromContent(
            """
            overrides:
                - extensions: [".cst"]
                  formatter: "csharp"
                  tabWidth: 2
                  useTabs: true
                  printWidth: 10
                  endOfLine: "LF"
            """
        );

        options.Overrides.Should().HaveCount(1);
        options.Overrides.First().Extensions.Should().HaveCount(1);
        options.Overrides.First().Extensions.First().Should().Be(".cst");
        options.Overrides.First().Formatter.Should().Be("csharp");
        options.Overrides.First().TabWidth.Should().Be(2);
        options.Overrides.First().UseTabs.Should().Be(true);
        options.Overrides.First().PrintWidth.Should().Be(10);
        options.Overrides.First().EndOfLine.Should().Be(EndOfLine.LF);
    }

    [Test]
    public void Should_Parse_Json_With_Overrides()
    {
        var options = ConfigurationFileParser.CreateFromContent(
            """
            {
                "overrides": [
                    {
                       "extensions": [".cst"],
                       "formatter": "csharp",
                       "tabWidth": 2,
                       "useTabs": true,
                       "printWidth": 10,
                       "endOfLine": "LF"
                    }
                ]
            }
            """
        );

        options.Overrides.Should().HaveCount(1);
        options.Overrides.First().Extensions.Should().HaveCount(1);
        options.Overrides.First().Extensions.First().Should().Be(".cst");
        options.Overrides.First().Formatter.Should().Be("csharp");
        options.Overrides.First().TabWidth.Should().Be(2);
        options.Overrides.First().UseTabs.Should().Be(true);
        options.Overrides.First().PrintWidth.Should().Be(10);
        options.Overrides.First().EndOfLine.Should().Be(EndOfLine.LF);
    }
}
