using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.Text;
using CSharpier.Cli.Options;
using CSharpier.Core;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli.Server;

internal class CSharpierServiceImplementation(ILogger logger)
{
    private readonly FileSystem fileSystem = new();

    private static readonly string[] configFileNames =
    [
        ".editorconfig",
        ".gitignore",
        ".csharpierignore",
    ];

    private readonly ConcurrentDictionary<
        string,
        (string ConfigStamp, OptionsProvider OptionsProvider)
    > optionsProvidersByDirectory = new(StringComparer.Ordinal);

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

            var optionsProvider = await this.GetOptionsProviderAsync(
                directoryName,
                cancellationToken
            );

            if (
                GeneratedCodeUtilities.IsGeneratedCodeFile(fileName)
                || await optionsProvider.IsFileIgnoredAsync(fileName, cancellationToken)
            )
            {
                return new FormatFileResult(Status.Ignored);
            }

            var printerOptions = await optionsProvider.GetPrinterOptionsForAsync(
                fileName,
                cancellationToken
            );
            if (printerOptions == null || printerOptions.Formatter is Formatter.Unknown)
            {
                return new FormatFileResult(Status.UnsupportedFile);
            }

            var result = await CodeFormatter.FormatAsync(
                formatFileParameter.fileContents,
                printerOptions,
                cancellationToken
            );

            if (result.ErrorDiagnostics.Any())
            {
                return new FormatFileResult(Status.Failed)
                {
                    errorMessage = "File had compilation errors and could not be formatted",
                };
            }

            if (string.IsNullOrEmpty(result.Code))
            {
                if (!string.IsNullOrEmpty(result.WarningMessage))
                {
                    return new FormatFileResult(Status.Failed)
                    {
                        errorMessage = result.WarningMessage,
                    };
                }

                if (!string.IsNullOrEmpty(result.FailureMessage))
                {
                    return new FormatFileResult(Status.Failed)
                    {
                        errorMessage = result.FailureMessage,
                    };
                }
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

    // building an OptionsProvider recompiles every ignore rule into a regex, so it is reused
    // across requests. the stamp covers the config files it was built from, so one being edited
    // while the server is running still takes effect on the next request
    private async Task<OptionsProvider> GetOptionsProviderAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        var configStamp = this.GetConfigStamp(directoryName);

        if (
            this.optionsProvidersByDirectory.TryGetValue(directoryName, out var cached)
            && cached.ConfigStamp == configStamp
        )
        {
            return cached.OptionsProvider;
        }

        var optionsProvider = await OptionsProvider.Create(
            directoryName,
            configPath: null,
            ignorePath: null,
            this.fileSystem,
            logger,
            cancellationToken
        );

        this.optionsProvidersByDirectory[directoryName] = (configStamp, optionsProvider);

        return optionsProvider;
    }

    private string GetConfigStamp(string directoryName)
    {
        var stamp = new StringBuilder();
        var directory = this.fileSystem.DirectoryInfo.New(directoryName);

        while (directory is not null)
        {
            if (directory.Exists)
            {
                foreach (var configFileName in configFileNames)
                {
                    this.AppendStamp(
                        stamp,
                        this.fileSystem.Path.Combine(directory.FullName, configFileName)
                    );
                }

                foreach (
                    var csharpierConfig in this.fileSystem.Directory.EnumerateFiles(
                        directory.FullName,
                        ".csharpierrc*",
                        SearchOption.TopDirectoryOnly
                    )
                )
                {
                    this.AppendStamp(stamp, csharpierConfig);
                }
            }

            directory = directory.Parent;
        }

        return stamp.ToString();
    }

    private void AppendStamp(StringBuilder stamp, string path)
    {
        var file = this.fileSystem.FileInfo.New(path);
        if (!file.Exists)
        {
            return;
        }

        stamp
            .Append(path)
            .Append('|')
            .Append(file.LastWriteTimeUtc.Ticks)
            .Append('|')
            .Append(file.Length)
            .Append(';');
    }
}
