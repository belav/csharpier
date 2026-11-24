using System.Management.Automation.Language;
using AwesomeAssertions;
using CSharpier.Core.PowerShell;

namespace CSharpier.Tests.PowerShell;

// The parser reports the DSC error only on a machine without DSC, so the error is built here with
// the id and position the parser reported in a Linux container.
public class PowerShellParseErrorsTests
{
    private const string DscExceptionText =
        "System.IO.DirectoryNotFoundException: Unable to find DSC schema store at /etc/opt/omi/conf/dsc/configuration. Please ensure PS DSC for Linux is installed.";

    [Test]
    [Arguments("configuration Sample { }\n", "Sample")]
    [Arguments("Configuration Sample { }\n", "Sample")]
    [Arguments("configuration <# c #> Sample { }\n", "Sample")]
    [Arguments("configuration `\n    Sample { }\n", "Sample")]
    [Arguments("configuration $name { }\n", "$name")]
    [Arguments("function f {\n    configuration Inner { }\n}\n", "Inner")]
    public void IsDscUnavailable_Should_Match_Parser_Error_On_Configuration_Name(
        string code,
        string name
    )
    {
        var tokens = Tokenize(code);
        var error = new ParseError(
            tokens.First(o => o.Text == name).Extent,
            "ParserError",
            DscExceptionText
        );

        PowerShellParseErrors.IsDscUnavailable(error, tokens).Should().BeTrue();
    }

    [Test]
    public void IsDscUnavailable_Should_Not_Match_Another_Error_Id_On_Configuration_Name()
    {
        var tokens = Tokenize("configuration Sample { }\n");
        var error = new ParseError(
            tokens.First(o => o.Text == "Sample").Extent,
            "UnexpectedToken",
            DscExceptionText
        );

        PowerShellParseErrors.IsDscUnavailable(error, tokens).Should().BeFalse();
    }

    [Test]
    [Arguments("configuration Sample { }\n", "{")]
    [Arguments("configuration Sample { }\n", "configuration")]
    [Arguments("function Sample { }\n", "Sample")]
    public void IsDscUnavailable_Should_Not_Match_Parser_Error_Elsewhere(string code, string text)
    {
        var tokens = Tokenize(code);
        var error = new ParseError(
            tokens.First(o => o.Text == text).Extent,
            "ParserError",
            DscExceptionText
        );

        PowerShellParseErrors.IsDscUnavailable(error, tokens).Should().BeFalse();
    }

    private static Token[] Tokenize(string code)
    {
        Parser.ParseInput(code, out var tokens, out _);
        return tokens;
    }
}
