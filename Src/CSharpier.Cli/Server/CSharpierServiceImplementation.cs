using System.IO.Abstractions;
using CSharpier.Cli.Options;
using CSharpier.Core;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli.Server;

internal class CSharpierServiceImplementation(ILogger logger)
{
    private readonly FileSystem fileSystem = new();

    public async Task<FormatFileResult> FormatFile(
        FormatFileParameter formatFileParameter,
        CancellationToken cancellationToken
    )
    {
        try
        {
            logger.LogInformation("Received request to format " + formatFileParameter.fileName);
            var fileName = this.fileSystem.Path.GetFullPath(formatFileParameter.fileName);
            if (
                formatFileParameter.fileContents.StartsWith("// csh-slow", StringComparison.Ordinal)
            )
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            if (
                formatFileParameter.fileContents.StartsWith(
                    "// csh-throw",
                    StringComparison.Ordinal
                )
            )
            {
                throw new Exception("Throwing because of // csh-throw comment");
            }
            var directoryName = this.fileSystem.Path.GetDirectoryName(fileName);
            DebugLogger.Log(directoryName ?? string.Empty);
            if (directoryName == null)
            {
                throw new Exception(
                    $"There was no directory found for file {formatFileParameter.fileName}"
                );
            }

            var optionsProvider = await OptionsProvider.Create(
                directoryName,
                configPath: null,
                ignorePath: null,
                this.fileSystem,
                logger,
                cancellationToken
            );

            if (
                GeneratedCodeUtilities.IsGeneratedCodeFile(fileName)
                || await optionsProvider.IsIgnoredAsync(fileName, cancellationToken)
            )
            {
                return new FormatFileResult(Status.Ignored);
            }

            var printerOptions = await optionsProvider.GetPrinterOptionsForAsync(
                formatFileParameter.fileName,
                cancellationToken
            );
            if (printerOptions == null || printerOptions.Formatter is Formatter.Unknown)
            {
                return new FormatFileResult(Status.UnsupportedFile);
            }

            // TODO #819 if there are compilation errors we need to do something here
            var result = await CodeFormatter.FormatAsync(
                formatFileParameter.fileContents,
                printerOptions,
                cancellationToken
            );

            if (result.CompilationErrors.Any())
            {
                return new FormatFileResult(Status.Failed)
                {
                    errorMessage = "File had compilation errors and could not be formatted",
                };
            }

            return new FormatFileResult(Status.Formatted) { formattedFile = result.Code };
        }
        catch (Exception ex)
        {
            DebugLogger.Log(ex.ToString());
            return new FormatFileResult(Status.Failed)
            {
                errorMessage = "An exception was thrown\n" + ex,
            };
        }
    }
}
