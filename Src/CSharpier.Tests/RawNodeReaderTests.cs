#pragma warning disable

using System.IO;
using System.Xml;
using System.Xml.Linq;
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
        var nodes = ReadAllNodes(xml);
        var attribute = nodes.First().Attributes.First();

        attribute.Name.Should().Be("Attribute");
        attribute.Value.Should().Be(attributeValue.Replace("\"", "&quot;"));
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
    public void XDocument(string xml, string attributeValue)
    {
        var xDoc = System.Xml.Linq.XDocument.Parse(xml);
        var element = xDoc.Root;
        var actualAttributeValue = element.Attribute("Attribute").Value;
        actualAttributeValue.Should().Be(attributeValue);
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
    public void XmlDocument(string xml, string attributeValue)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        var element = xmlDoc.DocumentElement;
        var actualAttributeValue = element.GetAttribute("Attribute");
        actualAttributeValue.Should().Be(attributeValue);
    }

    [Test]
    public void Should_Do_Stuff()
    {
        var xml = File.ReadAllText(
            @"c:\projects\csharpier-repos\mono\mcs\class\corlib\Documentation\en\System\Activator.xml"
        );

        ReadAllNodes(xml);
    }

    [Test]
    public void Should_Work_With_This_File_And_CRLF()
    {
        var xml = """
<?xml version="1.0"?>
<clause number="14.5.10.1" title="Object creation expressions">
  <paragraph
    >An <non_terminal where="14.5.10.1"
      >object-creation-expression</non_terminal
    > is used to create a new instance of a <non_terminal where="11.2"
      >class-type</non_terminal
    > or a <non_terminal where="11.1">value-type</non_terminal>. <grammar_production>
      <name>
        <non_terminal where="14.5.10.1">object-creation-expression</non_terminal>
      </name> : <rhs>
        <keyword>new</keyword>
        <non_terminal where="11">type</non_terminal>
        <terminal>(</terminal>
        <non_terminal where="14.4.1">argument-list</non_terminal>
        <opt />
        <terminal>)</terminal>
      </rhs>
    </grammar_production>
  </paragraph>
  <paragraph
    >The type of an <non_terminal where="14.5.10.1"
      >object-creation-expression</non_terminal
    > must be a <non_terminal where="11.2">class-type</non_terminal> or a <non_terminal where="11.1"
      >value-type</non_terminal
    >. The type cannot be an abstract <non_terminal where="11.2"
      >class-type</non_terminal
    >. </paragraph
  >
  <paragraph
    >The optional <non_terminal where="14.4.1"
      >argument-list</non_terminal
    > (<hyperlink>14.4.1</hyperlink>) is permitted only if the type is a <non_terminal where="11.2"
      >class-type</non_terminal
    > or a <non_terminal where="11.1">struct-type</non_terminal>. </paragraph
  >
  <paragraph
    >The compile-time processing of an <non_terminal where="14.5.10.1"
      >object-creation-expression</non_terminal
    > of the form new T(A), where T is a <non_terminal where="11.2"
      >class-type</non_terminal
    > or a <non_terminal where="11.1">value-type</non_terminal> and A is an optional <non_terminal
      where="14.4.1"
      >argument-list</non_terminal
    >, consists of the following steps: <list>
      <list_item
        > If T is a <non_terminal where="11.1"
          >value-type</non_terminal
        > and A is not present: </list_item
      >
      <list>
        <list_item
          > The <non_terminal where="14.5.10.1"
            >object-creation-expression</non_terminal
          > is a default constructor invocation. The result of the  <non_terminal where="14.5.10.1"
            >object-creation-expression</non_terminal
          > is a value of type T, namely the default value for T as defined in <hyperlink>11.1.1</hyperlink>. </list_item
        >
      </list>
      <list_item
        > Otherwise, if T is a <non_terminal where="11.2"
          >class-type</non_terminal
        > or a struct-type: </list_item
      >
      <list>
        <list_item
          > If T is an abstract <non_terminal where="11.2"
            >class-type</non_terminal
          >, a compile-time error occurs. </list_item
        >
        <list_item
          > The instance constructor to invoke is determined using the overload resolution rules of <hyperlink>14.4.2</hyperlink>. The set of candidate instance constructors consists of all accessible instance constructors declared in T. If the set of candidate instance constructors is empty, or if a single best instance constructor cannot be identified, a compile-time error occurs. </list_item
        >
        <list_item
          > The result of the <non_terminal where="14.5.10.1"
            >object-creation-expression</non_terminal
          > is a value of type T, namely the value produced by invoking the instance constructor determined in the step above. </list_item
        >
      </list>
      <list_item
        > Otherwise, the <non_terminal where="14.5.10.1"
          >object-creation-expression</non_terminal
        > is invalid, and a compile-time error occurs. </list_item
      >
    </list>
  </paragraph>
  <paragraph
    >The run-time processing of an <non_terminal where="14.5.10.1"
      >object-creation-expression</non_terminal
    > of the form new T(A), where T is <non_terminal where="11.2"
      >class-type</non_terminal
    > or a <non_terminal where="11.1">struct-type</non_terminal> and A is an optional <non_terminal
      where="14.4.1"
      >argument-list</non_terminal
    >, consists of the following steps: <list>
      <list_item> If T is a class-type: </list_item>
      <list>
        <list_item> A new instance of class T is allocated. If there is not enough memory available to allocate the new instance, a System.OutOfMemoryException is thrown and no further steps are executed. </list_item>
        <list_item
          > All fields of the new instance are initialized to their default values (<hyperlink>12.2</hyperlink>). </list_item
        >
        <list_item
          > The instance constructor is invoked according to the rules of function member invocation (<hyperlink>14.4.3</hyperlink>). A reference to the newly allocated instance is automatically passed to the instance constructor and the instance can be accessed from within that constructor as this. </list_item
        >
      </list>
      <list_item> If T is a struct-type: </list_item>
      <list>
        <list_item
          > An instance of type T is created by allocating a temporary local variable. Since an instance constructor of a <non_terminal
            where="11.1"
            >struct-type</non_terminal
          > is required to definitely assign a value to each field of the instance being created, no initialization of the temporary variable is necessary. </list_item
        >
        <list_item
          > The instance constructor is invoked according to the rules of function member invocation (<hyperlink>14.4.3</hyperlink>). A reference to the newly allocated instance is automatically passed to the instance constructor and the instance can be accessed from within that constructor as this. </list_item
        >
      </list>
    </list>
  </paragraph>
</clause>
""".Replace(Environment.NewLine, "\r\n");

        var nodes = ReadAllNodes(xml);
    }

    private static List<RawNode> ReadAllNodes(string xml)
    {
        return RawNodeReader.ReadAllNodes(xml, Environment.NewLine);
    }
}
