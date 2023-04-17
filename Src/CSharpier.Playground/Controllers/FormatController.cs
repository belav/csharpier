using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace CSharpier.Playground.Controllers;

using CSharpier.Formatters.Xml;

public class FormatResult
{
    public required string Code { get; set; }
    public required string Json { get; set; }
    public required string Doc { get; set; }
    public required List<FormatError> Errors { get; set; }
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

    [HttpPost]
    public async Task<FormatResult> Post([FromBody] string content, string fileExtension)
    {
        // TODO xml use the proper formatter class?
        if (fileExtension == "cs")
        {
            var result = await CSharpFormatter.FormatAsync(
                content,
                new PrinterOptions
                {
                    IncludeAST = true,
                    IncludeDocTree = true,
                    Width = PrinterOptions.WidthUsedByTests
                }
            );

            return new FormatResult
            {
                Code = result.Code,
                Json = result.AST,
                Doc = result.DocTree,
                Errors = result.CompilationErrors.Select(this.ConvertError).ToList(),
            };
        }

        if (fileExtension == "xml")
        {
            var result = XmlFormatter.Format(
                content,
                new PrinterOptions
                {
                    Width = PrinterOptions.WidthUsedByTests,
                    IncludeAST = true,
                    IncludeDocTree = true,
                }
            );

            return new FormatResult
            {
                Code = result.Code,
                Json = result.AST,
                Doc = result.DocTree,
                Errors = new List<FormatError>()
            };
        }

        throw new Exception("Cannot handle file extension " + fileExtension);
    }

    private FormatError ConvertError(Diagnostic diagnostic)
    {
        var lineSpan = diagnostic.Location.SourceTree!.GetLineSpan(diagnostic.Location.SourceSpan);
        return new FormatError { LineSpan = lineSpan, Description = diagnostic.ToString(), };
    }

    public string ExecuteApplication(string pathToExe, string workingDirectory, string args)
    {
        var processStartInfo = new ProcessStartInfo(pathToExe, args)
        {
            UseShellExecute = false,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            WorkingDirectory = workingDirectory,
            CreateNoWindow = true
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
