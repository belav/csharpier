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
    public void Should_Include_Doc_Tree_When_Requested()
    {
        var code = "if ($true) { Get-Item }\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
        {
            IncludeDocTree = true,
        };
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.DocTree.Should().Contain("Doc.Concat(");
        result.DocTree.Should().Contain("\"Get-Item\"");
    }

    [Test]
    public void Should_Not_Include_Doc_Tree_By_Default()
    {
        var code = "Get-Item\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.DocTree.Should().BeEmpty();
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

    // The parser keeps the line endings of the source in the value of a string, so this changes the
    // value. CSharpier does the same for C# verbatim and raw strings, and the validator compares
    // string values with \r\n read as \n.
    [Test]
    [Arguments(EndOfLine.LF, "\n")]
    [Arguments(EndOfLine.CRLF, "\r\n")]
    [Arguments(EndOfLine.Auto, "\r\n")]
    public async Task Should_Use_Configured_Line_Ending_Inside_Strings_With_Mixed_Line_Endings(
        EndOfLine endOfLine,
        string expectedLineEnding
    )
    {
        var code =
            "$a = @'\r\n  one\n  two\r\n'@\r\n"
            + "$b = @\"\n  one $c\r\n  two\n\"@\n"
            + "$d = 'one\r\n  two\n  three'\n"
            + "$e = \"one\n  two\r\n  three $c\"\r\n"
            + "<#\n  one\r\n  two\n#>\r\n"
            + "Write-Output 'one\r\n  two\n  three'\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
        {
            EndOfLine = endOfLine,
        };
        var result = await PowerShellFormatter.FormatAsync(code, options);

        result.Code.Should().Be(code.ReplaceLineEndings(expectedLineEnding));
        var validation = await new PowerShellFormattingValidator(code, result.Code).ValidateAsync(
            CancellationToken.None
        );
        validation.Failed.Should().BeFalse();
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
    [Arguments("\n")]
    [Arguments("\r\n")]
    [Arguments("  \n")]
    public void Should_Not_Format_Signed_Script(string endOfMarkerLine)
    {
        var code =
            "if($a){Write-Host 1}\n\n"
            + "# SIG # Begin signature block"
            + endOfMarkerLine
            + "# MIIFuQYJKoZIhvcNAQcCoIIFqjCCBaYCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB\n"
            + "# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR\n"
            + "# SIG # End signature block\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.Code.Should().Be(code);
        result
            .WarningMessage.Should()
            .Be(
                "Contains a signature block, which formatting would invalidate, so was not formatted."
            );
    }

    [Test]
    [Arguments("$a = @'\n# SIG # Begin signature block\n'@\n")]
    [Arguments("$a = '\n# SIG # Begin signature block\n'\n")]
    [Arguments("<#\n# SIG # Begin signature block\n#>\n")]
    [Arguments("# SIG # Begin signature block (copy)\n")]
    public async Task Should_Format_When_Signature_Marker_Is_Not_A_Whole_Comment(string marker)
    {
        var code = "if($a){Write-Host 1}\n" + marker;

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = await PowerShellFormatter.FormatAsync(code, options);

        result.WarningMessage.Should().BeEmpty();
        result.Code.Should().Be("if ($a) {\n    Write-Host 1\n}\n" + marker);
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
