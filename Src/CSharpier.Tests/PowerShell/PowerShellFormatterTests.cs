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

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public void Should_Use_One_Line_Ending_For_Multiline_Comments_And_Verbatim_Text(
        string endOfLine
    )
    {
        var code = """
            <#
              block comment
            #>
            $a = @"
            here string
            "@
            $b = @{
                One = 1
            }
            function Get-Value {
                    <#
                    .SYNOPSIS
                    #>
                Get-Item
            }

            """.ReplaceLineEndings("\r\n");

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
        {
            EndOfLine = endOfLine == "\n" ? EndOfLine.LF : EndOfLine.CRLF,
        };
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result
            .Code.Should()
            .Be(
                """
                <#
                  block comment
                #>
                $a = @"
                here string
                "@
                $b = @{
                    One = 1
                }
                function Get-Value {
                    <#
                    .SYNOPSIS
                    #>
                    Get-Item
                }

                """.ReplaceLineEndings(endOfLine)
            );
    }

    [Test]
    public void Should_Reindent_Lines_Of_Multiline_Comments()
    {
        var code = "function Get-Value {\n    <#\n        text  \n    #>\n    Get-Item\n}\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
        {
            UseTabs = true,
        };
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.Code.Should().Be("function Get-Value {\n\t<#\n\t    text\n\t#>\n\tGet-Item\n}\n");
    }

    [Test]
    public void Should_Not_Print_Type_Names_For_Nodes_Without_Printers()
    {
        var code = "switch ($a) {\n    1 { 'one' }\n}\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.Code.Should().Be(code);
    }
}
