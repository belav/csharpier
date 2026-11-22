using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Core.PowerShell;

internal static class PowerShellFormatter
{
    // Tests set these to reach paths the test machine cannot produce on demand: the parser on a
    // machine without DSC, and printer output that fails validation.
    internal static readonly AsyncLocal<bool> SimulateDscUnavailable = new();
    internal static readonly AsyncLocal<string?> SimulatedPrinterOutput = new();

    internal static Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions
    )
    {
        return FormatAsync(code, printerOptions, CancellationToken.None);
    }

    internal static Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        return LargeStack.Run(
            code.Length,
            () => Format(code, printerOptions, cancellationToken),
            cancellationToken
        );
    }

    private static CodeFormatterResult Format(
        string code,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ast = Parser.ParseInput(code, out var tokens, out var errors);

        if (HasSignatureBlock(tokens))
        {
            return new CodeFormatterResult
            {
                Code = code,
                WarningMessage =
                    "Contains a signature block, which formatting would invalidate, so was not formatted.",
                AST = printerOptions.IncludeAST ? AstSyntaxWriter.Write(ast) : string.Empty,
            };
        }

        if (
            SimulateDscUnavailable.Value
            || errors.Any(error => PowerShellParseErrors.IsDscUnavailable(error, tokens))
        )
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

        cancellationToken.ThrowIfCancellationRequested();

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

        cancellationToken.ThrowIfCancellationRequested();

        var formatted = SimulatedPrinterOutput.Value
            ?? DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

        return new CodeFormatterResult
        {
            Code = formatted,
            DocTree = printerOptions.IncludeDocTree ? DocSerializer.Serialize(doc) : string.Empty,
            AST = printerOptions.IncludeAST ? AstSyntaxWriter.Write(ast) : string.Empty,
        };
    }

    // An Authenticode signature covers the exact text of the script and is stored in comments,
    // from this line to "# SIG # End signature block", at the end of the file.
    private static bool HasSignatureBlock(Token[] tokens)
    {
        return tokens.Any(token =>
            token.Kind == TokenKind.Comment
            && token.Text.TrimEnd() == "# SIG # Begin signature block"
        );
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
