namespace CSharpier.Tests.FormattingTests;

public class CSharpFormatting : BaseTest
{
    [DynamicTestBuilder]
    public void BuildTests(DynamicTestBuilderContext context)
    {
        this.BuildTests<CSharpFormatting>(context, "cs");
    }
}
