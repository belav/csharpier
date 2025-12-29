namespace CSharpier.Tests.FormattingTests;

public class XmlIgnoreFormatting : BaseTest
{
    [DynamicTestBuilder]
    public void BuildTests(DynamicTestBuilderContext context)
    {
        this.BuildTests<XmlIgnoreFormatting>(context, "xml_ignore");
    }
}
