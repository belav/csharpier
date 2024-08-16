namespace CSharpier.Cli.Server;

using System.IO.Abstractions;
using CSharpier.Cli.Options;
using Microsoft.Extensions.Logging;

internal class CSharpierServiceImplementation(string? configPath, ILogger logger)
{
    private readonly IFileSystem fileSystem = new FileSystem();

    public async Task<FormatFileResult> FormatFile(
        FormatFileParameter formatFileParameter,
        CancellationToken cancellationToken
    )
    {
        try
        {
            logger.LogInformation("Received request to format " + formatFileParameter.fileName);
            var directoryName = this.fileSystem.Path.GetDirectoryName(formatFileParameter.fileName);
            DebugLogger.Log(directoryName ?? string.Empty);
            if (directoryName == null)
            {
                throw new Exception(
                    $"There was no directory found for file {formatFileParameter.fileName}"
                );
            }

            var optionsProvider = await OptionsProvider.Create(
                directoryName,
                configPath,
                this.fileSystem,
                logger,
                cancellationToken
            );

            if (
                GeneratedCodeUtilities.IsGeneratedCodeFile(formatFileParameter.fileName)
                || optionsProvider.IsIgnored(formatFileParameter.fileName)
            )
            {
                return new FormatFileResult(Status.Ignored);
            }

            var printerOptions = optionsProvider.GetPrinterOptionsFor(formatFileParameter.fileName);
            if (printerOptions == null)
            {
                return new FormatFileResult(Status.UnsupportedFile);
            }

            var result = await CSharpFormatter.FormatAsync(
                formatFileParameter.fileContents,
                printerOptions,
                cancellationToken
            );

            return new FormatFileResult(Status.Formatted) { formattedFile = result.Code };
        }
        catch (Exception ex)
        {
            DebugLogger.Log(ex.ToString());
            return new FormatFileResult(Status.Failed)
            {
                errorMessage = "An exception was thrown\n" + ex
            };
        }
    }
}
