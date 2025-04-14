using CSharpier.Core.Validators;
using CSharpier.Core.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.Validators;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class XmlFormattingValidatorTests
{
    [Test]
    public void Basic_Xml_Should_Pass_When_Indentation_Changes()
    {
        var left = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <LangVersion>4</LangVersion>
                </PropertyGroup>
            </Project>
            """;

        var right = """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <LangVersion>4</LangVersion>
              </PropertyGroup>
            </Project>
            """;

        var result = CompareSource(left, right);

        result.Failed.Should().BeFalse();
    }

    [Test]
    public void Should_Fail_With_Basic_Error()
    {
        var left = """
            <Project />
            """;

        var right = """
            <MyProject />
            """;

        var result = CompareSource(left, right);

        result.Failed.Should().BeTrue();
    }

    private static FormattingValidatorResult CompareSource(string left, string right)
    {
        return new XmlFormattingValidator(left, right).ValidateAsync(CancellationToken.None).Result;
    }
}
