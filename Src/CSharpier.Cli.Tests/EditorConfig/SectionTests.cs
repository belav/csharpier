using CSharpier.Cli.EditorConfig;
using FluentAssertions;
using IniParser.Model;
using NUnit.Framework;

namespace CSharpier.Cli.Tests.EditorConfig;

[TestFixture]
public class SectionTests
{
    [Test]
    public void Test1()
    {
        var path = "/test/test.cs";
        var result = new Section(new SectionData("*.cs"), "/test").IsMatch(path);

        result.Should().BeTrue();
    }

    [Test]
    public void Test2()
    {
        var path = "/test/test.cs";
        var result = new Section(new SectionData("*.{cs}"), "/test").IsMatch(path);

        result.Should().BeTrue();
    }

    [Test]
    public void Test3()
    {
        var path = "/test/test.cs";
        var result = new Section(new SectionData("*.{csx,cs}"), "/test").IsMatch(path);

        result.Should().BeTrue();
    }
}
