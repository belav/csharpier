using System.Management.Automation.Language;
using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.PowerShell;

namespace CSharpier.Tests.PowerShell;

public class PowerShellPrinterCoverageTests
{
    private static readonly Dictionary<string, string> HandledByBaseTypePrinter = new()
    {
        ["StringConstantExpressionAst"] = "ConstantExpression",
    };

    [Test]
    public void Every_Ast_Type_Should_Have_A_Printer()
    {
        var printerAssembly = typeof(PowerShellFormatter).Assembly;
        var astTypes = typeof(Ast)
            .Assembly.GetTypes()
            .Where(o => o.IsPublic && !o.IsAbstract && o.IsSubclassOf(typeof(Ast)))
            .ToList();

        astTypes.Should().NotBeEmpty();

        var missing = new List<string>();
        foreach (var astType in astTypes)
        {
            if (!HandledByBaseTypePrinter.TryGetValue(astType.Name, out var printerName))
            {
                printerName = astType.Name[..^"Ast".Length];
            }

            var printerType = printerAssembly.GetType(
                "CSharpier.Core.PowerShell.AstPrinters." + printerName
            );
            if (printerType is null)
            {
                missing.Add(astType.Name);
            }
        }

        missing.Should().BeEmpty();
    }

    [Test]
    public void Should_Use_One_Line_Ending_For_Mixed_Line_Endings()
    {
        var code = "if ($a) {\r\n    1\n}\n<#\r\n a\n#>\nfunction b {\n    param($c)\r\n}\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result
            .Code.Should()
            .Be(
                "if ($a) {\r\n    1\r\n}\r\n<#\r\n a\r\n#>\r\nfunction b {\r\n    param($c)\r\n}\r\n"
            );
    }

    [Test]
    public void Should_Format_When_Attribute_Type_Is_Not_Found()
    {
        var code = "[NotARealAttribute()]\nclass A { }\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.ErrorDiagnostics.Should().BeEmpty();
        result.Code.Should().Be(code);
    }

    [Test]
    public void Should_Keep_Braces_Of_Property_Names()
    {
        var code = "class A {\n    [string]${config-file}\n}\n";

        var options = new PrinterOptions(Formatter.PowerShell, XmlWhitespaceSensitivity.Strict);
        var result = PowerShellFormatter.FormatAsync(code, options).Result;

        result.Code.Should().Be(code);
    }
}
