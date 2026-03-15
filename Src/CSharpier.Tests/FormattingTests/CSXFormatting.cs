namespace CSharpier.Tests.FormattingTests;

public class CsxFormatting : BaseTest
{
    [DynamicTestBuilder]
    public void BuildTests(DynamicTestBuilderContext context)
    {
        this.BuildTests<CsxFormatting>(context, "csx");
    }
}
