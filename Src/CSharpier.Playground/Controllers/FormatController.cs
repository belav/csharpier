using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace CSharpier.Playground.Controllers;

public class FormatResult
{
    public required string Code { get; set; }
    public required string Json { get; set; }
    public required string Doc { get; set; }
    public required List<FormatError> Errors { get; set; }
    public required string SyntaxValidation { get; set; }
}

public class FormatError
{
    public required FileLinePositionSpan LineSpan { get; set; }
    public required string Description { get; set; }
}

[ApiController]
[Route("[controller]")]
public class FormatController : ControllerBase
{
    private readonly ILogger logger;

    public FormatController(ILogger<FormatController> logger)
    {
        this.logger = logger;
    }

    public class PostModel
    {
        public string Code { get; set; } = string.Empty;
        public int PrintWidth { get; set; }
        public int IndentSize { get; set; }
        public bool UseTabs { get; set; }
        public string Formatter { get; set; } = string.Empty;
    }

    [HttpPost]
    public async Task<FormatResult> Post(
        [FromBody] PostModel model,
        CancellationToken cancellationToken
    )
    {
        if (!Enum.TryParse<Formatter>(model.Formatter, ignoreCase: true, out var parsedFormatter))
        {
            throw new Exception("Invalid formatter " + model.Formatter);
        }

        var result = await CodeFormatter.FormatAsync(
            model.Code,
            new PrinterOptions
            {
                IncludeAST = true,
                IncludeDocTree = true,
                Width = model.PrintWidth,
                IndentSize = model.IndentSize,
                UseTabs = model.UseTabs,
                Formatter = parsedFormatter,
            },
            cancellationToken
        );

        // TODO xml what about this?
        var comparer = new SyntaxNodeComparer(
            model.Code,
            result.Code,
            result.ReorderedModifiers,
            result.ReorderedUsingsWithDisabledText,
            parsedFormatter is Formatter.CSharp ? SourceCodeKind.Regular : SourceCodeKind.Script,
            cancellationToken
        );

        return new FormatResult
        {
            Code = result.Code,
            Json = result.AST,
            Doc = result.DocTree,
            Errors = result.CompilationErrors.Select(this.ConvertError).ToList(),
            SyntaxValidation = await comparer.CompareSourceAsync(CancellationToken.None),
        };
    }

    private FormatError ConvertError(Diagnostic diagnostic)
    {
        var lineSpan = diagnostic.Location.SourceTree!.GetLineSpan(diagnostic.Location.SourceSpan);
        return new FormatError { LineSpan = lineSpan, Description = diagnostic.ToString() };
    }

    public string ExecuteApplication(string pathToExe, string workingDirectory, string args)
    {
        var processStartInfo = new ProcessStartInfo(pathToExe, args)
        {
            UseShellExecute = false,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            WorkingDirectory = workingDirectory,
            CreateNoWindow = true,
        };

        var process = Process.Start(processStartInfo);
        var output = process!.StandardError.ReadToEnd();
        process.WaitForExit();

        this.logger.LogInformation(
            "Output from '" + pathToExe + " " + args + "' was: " + Environment.NewLine + output
        );

        return output;
    }
}
