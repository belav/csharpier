#pragma warning disable

using System.Xml;
using CSharpier.Core.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
public class XmlReaderTest
{
    [Test]
    public void DoOtherStuff()
    {
        var xml = """
            <element></element>
            """;

        using var reader = XmlReader.Create(
            new StringReader(xml),
            new XmlReaderSettings { IgnoreWhitespace = false }
        );

        reader.Read();
    }

    [TestCase("<Element Attribute=\"x->x\"/>", "x->x")]
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
    public void DoStuff(string xml, string attributeValue)
    {
        using var reader = XmlReader.Create(
            new StringReader(xml),
            new XmlReaderSettings { IgnoreWhitespace = false }
        );

        reader.Read();
        // TODO the raw attribute reader should probably read all attributes and return them
        // TODO what if there are weird attribute values? can they be surrounded with a ' instead?
        // is there some other way to escape a "

        /* TODO
        if (reader.MoveToFirstAttribute())
        {
            do
            {
                Console.WriteLine($"Attribute {reader.Name} = {reader.Value}");
            }
            while (reader.MoveToNextAttribute());

            // Must move back to the element before continuing
            reader.MoveToElement();
        }
                */
        reader.MoveToFirstAttribute();
        var rawAttributeReader = new RawAttributeReader(xml, "\n");
        var attribute = rawAttributeReader.GetRawAttribute((IXmlLineInfo)reader, "Attribute");
        attribute.Should().Be(attributeValue.Replace("\r", string.Empty));
    }
}
