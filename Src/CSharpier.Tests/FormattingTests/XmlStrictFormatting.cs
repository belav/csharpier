namespace CSharpier.Tests.FormattingTests;

public class XmlStrictFormatting : BaseTest
{
    [DynamicTestBuilder]
    public void BuildTests(DynamicTestBuilderContext context)
    {
        this.BuildTests<XmlStrictFormatting>(context, "xml_strict");
    }
}
