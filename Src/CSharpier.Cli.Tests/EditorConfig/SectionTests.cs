using CSharpier.Cli.EditorConfig;
using FluentAssertions;
using IniParser.Model;
using NUnit.Framework;

namespace CSharpier.Cli.Tests.EditorConfig;

[TestFixture]
public class SectionTests
{
    [Test]
    public void BasicWildcardGlob()
    {
        var path = "/test/test.cs";
        var result = new Section(new SectionData("*.cs"), "/test").IsMatch(path, false);

        result.Should().BeTrue();
    }

    [Test]
    public void BasicGroupGlob()
    {
        var path = "/test/test.cs";
        var result = new Section(new SectionData("*.{cs}"), "/test").IsMatch(path, false);

        result.Should().BeTrue();
    }

    [Test]
    public void GroupWithTwoOptionsGlob()
    {
        var path = "/test/test.cs";
        var result = new Section(new SectionData("*.{csx,cs}"), "/test").IsMatch(path, false);

        result.Should().BeTrue();
    }
}
