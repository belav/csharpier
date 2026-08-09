using System.Xml;
using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.Xml;
using Node = CSharpier.Core.Xml.XNodePrinters.Node;

namespace CSharpier.Tests.Xml;

internal sealed class XmlNodePrinterTests
{
    [Test]
    public void Should_Format_Element_Ending_With_CSharpierIgnoreEnd()
    {
        var result = () => XmlFormatter.Format("<Root><A /><!-- csharpier-ignore-end --></Root>");

        result.Should().NotThrow();
    }

    [Test]
    [Arguments(" ")]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public void Should_Print_Whitespace_Only_Text_Node(string value)
    {
        var context = new XmlPrintingContext { NormalizedXml = value, LineEnding = "\n" };
        var textNode = new RawNode
        {
            NodeType = XmlNodeType.Text,
            Value = value,
            XmlWhitespaceSensitivity = XmlWhitespaceSensitivity.Ignore,
        };
        var parent = new RawNode
        {
            Name = "Root",
            NodeType = XmlNodeType.Element,
            XmlWhitespaceSensitivity = XmlWhitespaceSensitivity.Ignore,
            Nodes = [textNode],
        };
        textNode.Parent = parent;

        var result = () => Node.Print(textNode, context);

        result.Should().NotThrow();
    }
}
