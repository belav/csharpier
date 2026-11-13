using System.Management.Automation.Language;
using System.Text.Json;
using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.PowerShell;

namespace CSharpier.Tests.PowerShell;

public class PowerShellRobustnessTests
{
    private static readonly PrinterOptions Options = new(
        Formatter.PowerShell,
        XmlWhitespaceSensitivity.Strict
    );

    private static async Task<string> FormatAndValidate(string code)
    {
        var result = await PowerShellFormatter.FormatAsync(code, Options);
        result.ErrorDiagnostics.Should().BeEmpty();
        result.FailureMessage.Should().BeEmpty();

        var validation = await new PowerShellFormattingValidator(code, result.Code).ValidateAsync(
            CancellationToken.None
        );
        validation.FailureMessage.Should().BeNull();

        var secondResult = await PowerShellFormatter.FormatAsync(result.Code, Options);
        secondResult.Code.Should().Be(result.Code);

        return result.Code;
    }

    [Test]
    [Arguments("try { 1 } catch [System.IO.IOException], <# c #> [System.Exception] { 2 }\n")]
    [Arguments("try { 1 } catch [System.IO.IOException], # c\n  [System.Exception] { 2 }\n")]
    [Arguments("try { 1 } catch <# c #> { 2 } finally { 3 }\n")]
    public async Task Should_Print_Catch_Once_When_A_Catch_Clause_Is_Verbatim(string code)
    {
        var formatted = await FormatAndValidate(code);

        formatted.Should().NotContain("catch catch");
    }

    [Test]
    [Arguments("$j <# c #> = Write-Output Hi &\n")]
    [Arguments("$j = # why\n  Write-Output Hi &\n")]
    [Arguments("$j <# c #> = $k = Write-Output Hi &\n")]
    [Arguments("foo <# c #> -Name x &\n")]
    [Arguments("$j = a <# c #> && b &\n")]
    public async Task Should_Keep_Background_Operator_When_Statement_Is_Verbatim(string code)
    {
        var formatted = await FormatAndValidate(code);

        formatted.TrimEnd().Should().EndWith("&");
    }

    [Test]
    public async Task Should_Not_Change_Stop_Parsing_Argument_In_Front_Of_Pipe()
    {
        var code = "foo.exe --% raw args | here\n";

        var formatted = await FormatAndValidate(code);

        formatted.Should().Be(code);
    }

    [Test]
    public async Task Should_Trim_Stop_Parsing_Argument_At_End_Of_Line()
    {
        var formatted = await FormatAndValidate("foo.exe --% raw args   \nbar\n");

        formatted.Should().Be("foo.exe --% raw args\nbar\n");
    }

    [Test]
    [Arguments("function a` b { 1 }\n", "function a` b {")]
    [Arguments("function xyzzy`u{2195} { 1 }\n", "function xyzzy`u{2195} {")]
    [Arguments("function f`2 { 1 }\n", "function f`2 {")]
    [Arguments("filter global:G { 1 }\n", "filter global:G {")]
    public async Task Should_Print_Function_Name_As_Written(string code, string expectedStart)
    {
        var formatted = await FormatAndValidate(code);

        formatted.Should().StartWith(expectedStart);
    }

    [Test]
    public async Task Should_Not_Duplicate_Comment_In_Data_Statement_Header()
    {
        var formatted = await FormatAndValidate("data <# c #> { 'a' }\n");

        formatted.Should().Be("data {\n    <# c #>\n    'a'\n}\n");
    }

    [Test]
    public async Task Should_Not_Take_Property_Name_From_Comment()
    {
        var formatted = await FormatAndValidate(
            "class C {\n    [ValidateNotNull()] <# ${other} #> $Name\n    [int] <# $x #> ${a-b} = 1\n}\n"
        );

        formatted.Should().Contain("$Name");
        formatted.Should().Contain("${a-b} = 1");
        formatted.Should().NotContain("#>\n    ${other}");
    }

    [Test]
    public async Task Should_Not_Take_Switch_Flag_From_Comment()
    {
        var formatted = await FormatAndValidate(
            "switch <# -Regex #> ($x) { 1 { 2 } }\nswitch -Wildcard <# -Exact #> ($x) { 1 { 2 } }\n"
        );

        formatted.Should().NotContain("switch -Regex");
        formatted.Should().NotContain("-Exact (");
        formatted.Should().Contain("switch -Wildcard (");
    }

    [Test]
    [Arguments("$a = - <# c #> 1\n")]
    [Arguments("$i <# c #> ++\n")]
    [Arguments("cmd -Name: <# -Other: #> $v\n")]
    [Arguments("cmd 2> <# 3> #> out.txt\n")]
    [Arguments("$o.Method(<# ( #> 1)\n")]
    [Arguments("function f <# ) #> { 1 }\n")]
    [Arguments("data foo -supportedCommand <# x #> ConvertTo-Json { 'b' }\n")]
    public async Task Should_Not_Read_Syntax_From_Comments(string code)
    {
        await FormatAndValidate(code);
    }

    [Test]
    public async Task Should_Keep_Script_Help_Apart_From_First_Function()
    {
        var code = "<#\n.SYNOPSIS\nScript level help.\n#>\n\n\n\nfunction Get-Foo { 'foo' }\n";

        var formatted = await FormatAndValidate(code);

        formatted
            .Should()
            .StartWith("<#\n.SYNOPSIS\nScript level help.\n#>\n\n\nfunction Get-Foo {");
        HasFunctionHelp(code).Should().BeFalse();
        HasFunctionHelp(formatted).Should().BeFalse();
    }

    [Test]
    public async Task Should_Keep_Function_Help_Attached_To_Function()
    {
        var code = "<#\n.SYNOPSIS\nFunction help.\n#>\n\nfunction Get-Foo { 'foo' }\n";

        var formatted = await FormatAndValidate(code);

        HasFunctionHelp(code).Should().BeTrue();
        HasFunctionHelp(formatted).Should().BeTrue();
    }

    [Test]
    public async Task Should_Collapse_Blank_Lines_After_Ordinary_Comment()
    {
        var formatted = await FormatAndValidate("# plain\n\n\n\nfunction Get-Foo { 'foo' }\n");

        formatted.Should().StartWith("# plain\n\nfunction Get-Foo {");
    }

    private static bool HasFunctionHelp(string code)
    {
        var ast = Parser.ParseInput(code, out _, out _);
        var function = (FunctionDefinitionAst)ast.Find(o => o is FunctionDefinitionAst, false);
        return function.GetHelpContent() is not null;
    }

    [Test]
    [Arguments("chain")]
    [Arguments("not")]
    [Arguments("parens")]
    [Arguments("if")]
    public async Task Should_Fail_Instead_Of_Overflowing_On_Deep_Nesting(string kind)
    {
        var code = kind switch
        {
            "chain" => "$a = $b" + string.Concat(Enumerable.Repeat(".c()", 1000)) + "\n",
            "not" => "$a = " + string.Concat(Enumerable.Repeat("-not ", 1000)) + "$b\n",
            "parens" => "$a = " + new string('(', 500) + "1" + new string(')', 500) + "\n",
            _ => string.Concat(Enumerable.Repeat("if (1) { ", 1000))
                + "1"
                + string.Concat(Enumerable.Repeat(" }", 1000))
                + "\n",
        };

        var result = await PowerShellFormatter.FormatAsync(code, Options);

        result.Code.Should().Be(code);
        result.FailureMessage.Should().Be("We can't handle this deep of recursion yet.");
    }

    [Test]
    public async Task Should_Format_And_Validate_Long_Operator_Chain()
    {
        var code = "$a = " + string.Concat(Enumerable.Repeat("1 +  ", 20000)) + "1\n";

        var result = await PowerShellFormatter.FormatAsync(code, Options);
        result.FailureMessage.Should().BeEmpty();

        var validation = await new PowerShellFormattingValidator(code, result.Code).ValidateAsync(
            CancellationToken.None
        );
        validation.Failed.Should().BeFalse();
    }

    [Test]
    public async Task Should_Write_Ast_For_Nested_Code()
    {
        var code = "$a = " + new string('(', 60) + "1" + new string(')', 60) + "\n";

        var result = await PowerShellFormatter.FormatAsync(
            code,
            new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
            {
                IncludeAST = true,
            }
        );

        result.AST.Should().Contain("ParenExpressionAst");
    }

    [Test]
    public async Task Should_Write_Tuples_In_Ast_As_Nodes()
    {
        var result = await PowerShellFormatter.FormatAsync(
            "if ($a) { 1 }\n@{ Key = 'Value' }\n",
            new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict)
            {
                IncludeAST = true,
            }
        );

        using var document = JsonDocument.Parse(
            result.AST,
            new JsonDocumentOptions { MaxDepth = 1000 }
        );
        result.AST.Should().NotContain("System.Tuple");
        result.AST.Should().Contain("StatementBlockAst");
        result.AST.Should().Contain("\"Key\"");
    }
}
