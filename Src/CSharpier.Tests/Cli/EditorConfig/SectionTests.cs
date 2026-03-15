using AwesomeAssertions;
using CSharpier.Cli.EditorConfig;
using IniParser.Model;

namespace CSharpier.Tests.Cli.EditorConfig;

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
