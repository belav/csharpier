#pragma warning disable

using System.Xml;
using CSharpier.Core.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
public class RawNodeReaderTests
{
    [Test]
    public void Should_Read_Text()
    {
        var xml = """
            <Text>text</Text>
            """;

        var elements = ReadAllElements(xml);
        elements.Count.Should().Be(1);
        elements[0].Nodes.Count.Should().Be(1);
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

        var elements = ReadAllElements(xml);
        elements.Count.Should().Be(2);
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

        var elements = ReadAllElements(xml);
        elements.Count.Should().Be(1);
        elements[0].Nodes.Count.Should().Be(3);
        var first = elements[0].Nodes[0];
        var second = elements[0].Nodes[1];
        var third = elements[0].Nodes[2];

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

        var elements = ReadAllElements(xml);

        var attributes = elements.First().Attributes;

        attributes.Length.Should().Be(2);
        attributes[0].Name.Should().Be("one");
        attributes[0].Value.Should().Be("1");
        attributes[1].Name.Should().Be("two");
        attributes[1].Value.Should().Be("2");
    }

    [TestCase("<Element Attribute=\"x->x\"/>", "x->x")]
    [TestCase("<Element Attribute='SomeText\"'/>", "SomeText\"")]
    [TestCase("<Element Attribute=\"@('', '&#xA;')\" />", "@('', '&#xA;')")]
    [TestCase("<Element Attribute=\"@('', '&#xA;')\" />", "@('', '&#xA;')")]
    [TestCase(
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
    public void Should_Read_Various_Attributes(string xml, string attributeValue)
    {
        var elements = ReadAllElements(xml);
        var attribute = elements.First().Attributes.First();

        attribute.Name.Should().Be("Attribute");
        attribute.Value.Should().Be(attributeValue.Replace("\"", "&quote;"));
    }

    private static List<RawNode> ReadAllElements(string xml)
    {
        using var reader = XmlReader.Create(
            new StringReader(xml),
            new XmlReaderSettings { IgnoreWhitespace = false }
        );

        return RawNodeReader.ReadAllNodes(xml, Environment.NewLine, reader);
    }
}
