using AwesomeAssertions;
using CSharpier.Core.PowerShell;

namespace CSharpier.Tests.PowerShell;

public class PowerShellFormattingValidatorTests
{
    [Test]
    [Arguments("If ($a -EQ 1) { Get-Item }", "if ($a -eq 1) {\n    Get-Item\n}\n")]
    [Arguments("Get-Item; Get-Other;", "Get-Item\nGet-Other\n")]
    [Arguments("Get-Item `\n    -Path $a", "Get-Item -Path $a\n")]
    [Arguments("Get-Item |\n    Sort-Object", "Get-Item | Sort-Object\n")]
    [Arguments("$a=1+2", "$a = 1 + 2\n")]
    [Arguments("Get-Item\n\n\n\nGet-Other", "Get-Item\n\nGet-Other\n")]
    [Arguments("  # comment\nGet-Item   # other", "# comment\nGet-Item # other\n")]
    [Arguments(
        "<#\n    .SYNOPSIS\n  text\n  #>\nGet-Item",
        "<#\n.SYNOPSIS\n    text\n#>\nGet-Item\n"
    )]
    [Arguments("$a = @\"\r\none\r\ntwo\r\n\"@\r\n", "$a = @\"\none\ntwo\n\"@\n")]
    [Arguments(
        "TRY { Get-Item } CATCH { } FINALLY { }",
        "try {\n    Get-Item\n} catch { } finally { }\n"
    )]
    public async Task Should_Pass_When_Only_Formatting_Changed(string original, string formatted)
    {
        var result = await new PowerShellFormattingValidator(original, formatted).ValidateAsync(
            CancellationToken.None
        );

        result.FailureMessage.Should().BeNullOrEmpty();
        result.Failed.Should().BeFalse();
    }

    [Test]
    [Arguments("Get-Item # comment", "Get-Item")]
    [Arguments("Get-Item", "Get-Item # comment")]
    [Arguments("Get-Item # comment", "Get-Item # other comment")]
    [Arguments("Get-Item\nGet-Other", "Get-Item")]
    [Arguments("Get-Item -Path $a", "Get-Item $a")]
    [Arguments("Get-Item -Path $a", "Get-Item -path $a")]
    [Arguments("Get-Item $a $b", "Get-Item $a")]
    [Arguments("$a -eq 1", "$a -ne 1")]
    [Arguments("$a -eq 1", "$a -ceq 1")]
    [Arguments("$a = 1", "$a += 1")]
    [Arguments("$a = 0x10", "$a = 17")]
    [Arguments("$a = 'value'", "$a = \"value\"")]
    [Arguments("$a = 'one'", "$a = 'two'")]
    [Arguments("$a = \"one $b\"", "$a = \"one  $b\"")]
    [Arguments("$a = @'\none\n  two\n'@", "$a = @'\none\ntwo\n'@")]
    [Arguments("$a", "$b")]
    [Arguments("$env:a", "$a")]
    [Arguments("[int]$a", "[string]$a")]
    [Arguments("& $a", ". $a")]
    [Arguments("& $a", "$a")]
    [Arguments("$a.b()", "$a::b()")]
    [Arguments("$a.b", "$a.c")]
    [Arguments("Get-Item | Sort-Object", "Get-ItemSort-Object")]
    [Arguments("Get-Item > out.txt", "Get-Item")]
    [Arguments("try { } catch [System.IO.IOException] { }", "try { } catch { }")]
    [Arguments(":outer foreach ($a in $b) { }", "foreach ($a in $b) { }")]
    [Arguments("function a($b) { }", "function a { }")]
    [Arguments("function a { process { $_ } }", "function a { $_ }")]
    [Arguments("if ($a) { } else { }", "if ($a) { }")]
    [Arguments("Get-Item", "Get-Item (")]
    public async Task Should_Fail_When_Code_Changed(string original, string formatted)
    {
        var result = await new PowerShellFormattingValidator(original, formatted).ValidateAsync(
            CancellationToken.None
        );

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().NotBeNullOrEmpty();
    }
}
