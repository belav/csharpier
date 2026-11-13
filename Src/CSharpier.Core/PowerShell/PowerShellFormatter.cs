using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Core.PowerShell;

internal static class PowerShellFormatter
{
    internal static Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions
    )
    {
        return LargeStack.Run(code.Length, () => Format(code, printerOptions));
    }

    private static CodeFormatterResult Format(string code, PrinterOptions printerOptions)
    {
        var ast = Parser.ParseInput(code, out var tokens, out var errors);

        if (errors.Any(PowerShellParseErrors.IsDscUnavailable))
        {
            return new CodeFormatterResult
            {
                Code = code,
                WarningMessage =
                    "Contains a DSC configuration, which cannot be parsed on this machine because DSC is not installed, so was not formatted.",
                AST = printerOptions.IncludeAST ? AstSyntaxWriter.Write(ast) : string.Empty,
            };
        }

        errors = errors.Where(PowerShellParseErrors.IsBlocking).ToArray();
        if (errors.Length != 0)
        {
            var sourceText = SourceText.From(code);
            return new CodeFormatterResult
            {
                Code = code,
                ErrorDiagnostics = errors
                    .Select(error => CreateDiagnosticFromParseError(sourceText, error))
                    .ToList(),
                AST = printerOptions.IncludeAST ? AstSyntaxWriter.Write(ast) : string.Empty,
            };
        }

        var lineEnding = PrinterOptions.GetLineEnding(code, printerOptions);

        Doc doc;
        try
        {
            doc = PowerShellPrinter.Print(ast, tokens, code);
        }
        catch (InTooDeepException)
        {
            return new CodeFormatterResult
            {
                Code = code,
                FailureMessage = "We can't handle this deep of recursion yet.",
            };
        }

        var formatted = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

        return new CodeFormatterResult
        {
            Code = formatted,
            AST = printerOptions.IncludeAST ? AstSyntaxWriter.Write(ast) : string.Empty,
        };
    }

    private static Diagnostic CreateDiagnosticFromParseError(
        SourceText sourceText,
        ParseError error
    )
    {
        var extent = error.Extent;

        var start = Math.Clamp(extent.StartOffset, 0, sourceText.Length);
        var end = Math.Clamp(extent.EndOffset, start, sourceText.Length);
        var span = new TextSpan(start, end - start);

        var location = Location.Create(
            filePath: string.Empty,
            textSpan: span,
            lineSpan: sourceText.Lines.GetLinePositionSpan(span)
        );

        var descriptor = new DiagnosticDescriptor(
            id: "PS001",
            title: "PowerShell parsing error",
            messageFormat: "{0}",
            category: "PowerShell",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(descriptor, location, error.Message);
    }
}
