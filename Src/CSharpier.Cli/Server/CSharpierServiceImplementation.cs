namespace CSharpier.Cli.Server;

using System.IO.Abstractions;
using CSharpier.Cli.Options;

internal class CSharpierServiceImplementation
{
    private readonly string? configPath;
    private readonly IFileSystem fileSystem;
    private readonly ConsoleLogger logger;

    public CSharpierServiceImplementation(string? configPath, ConsoleLogger logger)
    {
        this.configPath = configPath;
        this.logger = logger;
        this.fileSystem = new FileSystem();
    }

    public async Task<FormatFileResult> FormatFile(
        FormatFileParameter formatFileParameter,
        CancellationToken cancellationToken
    )
    {
        try
        {
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
                this.configPath,
                this.fileSystem,
                this.logger,
                cancellationToken
            );

            if (
                GeneratedCodeUtilities.IsGeneratedCodeFile(formatFileParameter.fileName)
                || optionsProvider.IsIgnored(formatFileParameter.fileName)
            )
            {
                return new FormatFileResult(Status.Ignored);
            }

            var result = await CSharpFormatter.FormatAsync(
                formatFileParameter.fileContents,
                optionsProvider.GetPrinterOptionsFor(formatFileParameter.fileName),
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
