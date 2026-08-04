using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.PowerShell;

namespace CSharpier.Tests.PowerShell;

public class PowerShellFormatterTests
{
    [Test]
    [Arguments("script.ps1")]
    [Arguments("module.psm1")]
    [Arguments("manifest.psd1")]
    [Arguments("SCRIPT.PS1")]
    public void GetFormatter_Recognizes_PowerShell_Extensions(string fileName)
    {
        PrinterOptions.GetFormatter(fileName).Should().Be(Formatter.PowerShell);
    }

    [Test]
    public void Should_Report_Errors()
    {
        var code = "function { broken (";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.Code.Should().Be(code);
        result
            .ErrorDiagnostics.First()
            .ToString()
            .Should()
            .Be("(1,9): error PS001: Missing name after function keyword.");
    }

    [Test]
    public void Should_Include_Ast_When_Requested()
    {
        var code = "if ($true) { Get-Item }\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
        {
            IncludeAST = true,
        };
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.AST.Should().Contain("ScriptBlockAst");
        result.AST.Should().Contain("IfStatementAst");
    }

    [Test]
    public void Should_Include_Ast_For_Unparsable_Code_When_Requested()
    {
        var code = "function { broken (";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
        {
            IncludeAST = true,
        };
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.ErrorDiagnostics.Should().NotBeEmpty();
        result.AST.Should().Contain("ScriptBlockAst");
    }

    [Test]
    public void Should_Not_Include_Ast_By_Default()
    {
        var code = "Get-Item\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.AST.Should().BeEmpty();
    }
}
