using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.PowerShell;

namespace CSharpier.Tests.PowerShell;

public class PowerShellResolutionErrorTests
{
    private const string CodeWithMissingModule = """
        configuration Sample {
            Import-DscResource -ModuleName ModuleThatIsNotInstalled
            node localhost {
                $x=1
            }
        }

        """;

    [Test]
    public async Task Should_Format_When_A_Module_Cannot_Be_Resolved()
    {
        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = await PowerShellFormatter.FormatAsync(
            "using module ModuleThatIsNotInstalled\n$x=1\n",
            options
        );

        result.ErrorDiagnostics.Should().BeEmpty();
    }

    [Test]
    public async Task Should_Format_When_A_Dsc_Resource_Cannot_Be_Resolved()
    {
        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = await PowerShellFormatter.FormatAsync(CodeWithMissingModule, options);

        result.ErrorDiagnostics.Should().BeEmpty();
    }

    [Test]
    public async Task Should_Format_When_A_Type_Cannot_Be_Resolved()
    {
        var code = "class Sample : TypeThatDoesNotExist {\n}\n$x=1\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = await PowerShellFormatter.FormatAsync(code, options);

        result.ErrorDiagnostics.Should().BeEmpty();
        result.Code.Should().Contain("$x = 1");
    }

    [Test]
    public async Task Should_Still_Report_Syntax_Errors_Next_To_Resolution_Errors()
    {
        var code = "class Sample : TypeThatDoesNotExist {\n}\nfunction { broken (";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = await PowerShellFormatter.FormatAsync(code, options);

        result.Code.Should().Be(code);
        result.ErrorDiagnostics.Should().NotBeEmpty();
    }

    [Test]
    public async Task Validator_Should_Ignore_Resolution_Errors()
    {
        var original = "class Sample : TypeThatDoesNotExist {\n}\n$x=1\n";
        var formatted = "class Sample : TypeThatDoesNotExist {\n}\n$x = 1\n";

        var result = await new PowerShellFormattingValidator(original, formatted).ValidateAsync(
            CancellationToken.None
        );

        result.Failed.Should().BeFalse();
    }
}
