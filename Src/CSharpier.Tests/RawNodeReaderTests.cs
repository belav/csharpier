#pragma warning disable

using AwesomeAssertions;
using CSharpier.Core.Xml;

namespace CSharpier.Tests;

public class RawNodeReaderTests
{
    [Test]
    [Skip("TODO 1599 make this work when keeping lines is working")]
    public void Should_Keep_Only_Whitespace()
    {
        var xml = "<JustWhitespace> </JustWhitespace>";

        var nodes = ReadAllNodes(xml);
        nodes.First().Name.Should().Be("JustWhitespace");
        nodes.First().Nodes.Count.Should().Be(1);
    }

    [Test]
    public void Should_Read_Text()
    {
        var xml = """
            <Text>text</Text>
            """;

        var nodes = ReadAllNodes(xml);
        nodes.Count.Should().Be(1);
        nodes[0].Nodes.Count.Should().Be(1);
    }

    [Test]
    public void Should_Read_Comments()
    {
        var xml = """
            <!-- comment -->
            <Root>
              <!-- comment -->
              <Element />
            </Root>
            """;

        var nodes = ReadAllNodes(xml);
        nodes.Count.Should().Be(2);
    }

    [Test]
    public void Should_Populate_Previous_And_Next()
    {
        var xml = """
            <Root>
              <First />
              <Second />
              <Third />
            </Root>
            """;

        var nodes = ReadAllNodes(xml);
        nodes.Count.Should().Be(1);
        nodes[0].Nodes.Count.Should().Be(3);
        var first = nodes[0].Nodes[0];
        var second = nodes[0].Nodes[1];
        var third = nodes[0].Nodes[2];

        first.NextNode.Should().Be(second);
        first.PreviousNode.Should().BeNull();
        second.NextNode.Should().Be(third);
        second.PreviousNode.Should().Be(first);
        third.NextNode.Should().Be(null);
        third.PreviousNode.Should().Be(second);
    }

    [Test]
    public void Should_Read_Attributes()
    {
        var xml = """
            <element one="1" two="2" />
            """;

        var nodes = ReadAllNodes(xml);

        var attributes = nodes.First().Attributes;

        attributes.Length.Should().Be(2);
        attributes[0].Name.Should().Be("one");
        attributes[0].Value.Should().Be("1");
        attributes[1].Name.Should().Be("two");
        attributes[1].Value.Should().Be("2");
    }

    [Arguments("<Element Attribute=\"x->x\"/>", "x->x")]
    [Arguments("<Element Attribute='SomeText\"'/>", "SomeText\"")]
    [Arguments("<Element Attribute=\"@('', '&#xA;')\" />", "@('', '&#xA;')")]
    [Arguments("<Element Attribute=\"@('', '&#xA;')\" />", "@('', '&#xA;')")]
    [Arguments(
        """
            <Element
              Attribute=" '$(MSBuildProjectName)' != 'Microsoft.TestCommon'
                AND '$(MSBuildProjectName)' != 'System.Net.Http.Formatting.NetCore.Test'
                AND '$(MSBuildProjectName)' != 'System.Net.Http.Formatting.NetStandard.Test' "
            />
            """,
        """
             '$(MSBuildProjectName)' != 'Microsoft.TestCommon'
                AND '$(MSBuildProjectName)' != 'System.Net.Http.Formatting.NetCore.Test'
                AND '$(MSBuildProjectName)' != 'System.Net.Http.Formatting.NetStandard.Test' 
            """
    )]

    [Test]
    public void Should_Read_Various_Attributes(string xml, string attributeValue)
    {
        var nodes = ReadAllNodes(xml);
        var attribute = nodes.First().Attributes.First();

        attribute.Name.Should().Be("Attribute");
        attribute.Value.Should().Be(attributeValue.Replace("\"", "&quot;"));
    }

    private static List<RawNode> ReadAllNodes(string xml)
    {
        return RawNodeReader.ParseXml(xml, Environment.NewLine).Nodes;
    }
}
