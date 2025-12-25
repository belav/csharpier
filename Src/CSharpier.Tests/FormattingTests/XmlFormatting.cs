namespace CSharpier.Tests.FormattingTests;

public class XmlFormatting : BaseTest
{
    [DynamicTestBuilder]
    public void BuildTests(DynamicTestBuilderContext context)
    {
        this.BuildTests<XmlFormatting>(context, "xml");
    }
}
