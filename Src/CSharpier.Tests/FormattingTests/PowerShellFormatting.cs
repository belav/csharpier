namespace CSharpier.Tests.FormattingTests;

public class PowerShellFormatting : BaseTest
{
    [DynamicTestBuilder]
    public void BuildTests(DynamicTestBuilderContext context)
    {
        this.BuildTests<PowerShellFormatting>(context, "powershell");
    }
}
