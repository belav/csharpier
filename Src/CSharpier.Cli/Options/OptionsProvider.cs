using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Cli.EditorConfig;
using CSharpier.Core;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli.Options;

internal class OptionsProvider
{
    private readonly ConcurrentDictionary<string, EditorConfigSections?> editorConfigByDirectory =
        new();
    private readonly ConcurrentDictionary<
        string,
        CSharpierConfigData?
    > csharpierConfigsByDirectory = new();
    private readonly ConcurrentDictionary<string, IgnoreFile?> ignoreFilesByDirectory = new();
    private readonly ConfigurationFileOptions? specifiedConfigFile;
    private readonly EditorConfigSections? specifiedEditorConfig;
    private readonly IFileSystem fileSystem;
    private readonly ILogger logger;

    private OptionsProvider(
        ConfigurationFileOptions? specifiedPrinterOptions,
        EditorConfigSections? specifiedEditorConfig,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        this.specifiedConfigFile = specifiedPrinterOptions;
        this.specifiedEditorConfig = specifiedEditorConfig;
        this.fileSystem = fileSystem;
        this.logger = logger;
    }

    public static async ValueTask<OptionsProvider> Create(
        string directoryName,
        string? configPath,
        string? ignorePath,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        var csharpierConfigPath = configPath;
        string? editorConfigPath = null;

        if (configPath is not null && Path.GetFileName(configPath) == ".editorconfig")
        {
            csharpierConfigPath = null;
            editorConfigPath = configPath;
        }

        var specifiedConfigFile = csharpierConfigPath is not null
            ? CSharpierConfigParser.Create(csharpierConfigPath, fileSystem, logger)
            : null;

        var ignoreFile = await IgnoreFile.CreateAsync(
            directoryName,
            fileSystem,
            ignorePath,
            cancellationToken
        );

#pragma warning disable IDE0270
        if (ignoreFile is null)
        {
            // should never happen
            throw new Exception("Unable to locate an IgnoreFile for " + directoryName);
        }
#pragma warning restore IDE0270

        var specifiedEditorConfig = editorConfigPath is not null
            ? await EditorConfigLocator.FindForDirectoryNameAsync(
                Path.GetDirectoryName(editorConfigPath)!,
                fileSystem,
                ignoreFile,
                cancellationToken
            )
            : null;

        var optionsProvider = new OptionsProvider(
            specifiedConfigFile,
            specifiedEditorConfig,
            fileSystem,
            logger
        );

        optionsProvider.ignoreFilesByDirectory[directoryName] = ignoreFile;

        if (csharpierConfigPath is null)
        {
            optionsProvider.csharpierConfigsByDirectory[directoryName] =
                CSharpierConfigParser.FindForDirectoryName(directoryName, fileSystem, logger);
        }

        if (editorConfigPath is null)
        {
            optionsProvider.editorConfigByDirectory[directoryName] =
                await EditorConfigLocator.FindForDirectoryNameAsync(
                    directoryName,
                    fileSystem,
                    ignoreFile,
                    cancellationToken
                );
        }

        return optionsProvider;
    }

    public async ValueTask<PrinterOptions?> GetPrinterOptionsForAsync(
        string filePath,
        CancellationToken cancellationToken
    )
    {
        if (this.specifiedConfigFile is not null)
        {
            return this.specifiedConfigFile.ConvertToPrinterOptions(filePath);
        }

        if (this.specifiedEditorConfig is not null)
        {
            return this.specifiedEditorConfig.ConvertToPrinterOptions(filePath, true);
        }

        var directoryName = this.fileSystem.Path.GetDirectoryName(filePath);

        ArgumentNullException.ThrowIfNull(directoryName);

        var resolvedCSharpierConfig = await this.FindCSharpierConfigAsync(directoryName);
        if (resolvedCSharpierConfig is not null)
        {
            return resolvedCSharpierConfig.CSharpierConfig.ConvertToPrinterOptions(filePath);
        }

        var resolvedEditorConfig = await this.FindEditorConfigAsync(
            directoryName,
            cancellationToken
        );
        if (resolvedEditorConfig is not null)
        {
            return resolvedEditorConfig.ConvertToPrinterOptions(filePath, false);
        }

        var formatter = PrinterOptions.GetFormatter(filePath);
        return formatter != Formatter.Unknown ? new PrinterOptions(formatter) : null;
    }

    private ValueTask<CSharpierConfigData?> FindCSharpierConfigAsync(string directoryName)
    {
        return this.FindFileAsync(
            directoryName,
            this.csharpierConfigsByDirectory,
            (searchingDirectory, cancellationToken) =>
                this
                    .fileSystem.Directory.EnumerateFiles(
                        searchingDirectory,
                        ".csharpierrc*",
                        SearchOption.TopDirectoryOnly
                    )
                    .Any(),
            (searchingDirectory, cancellationToken) =>
                Task.FromResult(
                    CSharpierConfigParser.FindForDirectoryName(
                        searchingDirectory,
                        this.fileSystem,
                        this.logger
                    )
                ),
            CancellationToken.None
        );
    }

    private async ValueTask<EditorConfigSections?> FindEditorConfigAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        return await this.FindFileAsync(
            directoryName,
            this.editorConfigByDirectory,
            (searchingDirectory, cancellationToken) =>
                this.fileSystem.File.Exists(Path.Combine(searchingDirectory, ".editorconfig")),
            async (searchingDirectory, cancellationToken) =>
                await EditorConfigLocator.FindForDirectoryNameAsync(
                    searchingDirectory,
                    this.fileSystem,
                    await this.FindIgnoreFileAsync(searchingDirectory, cancellationToken),
                    cancellationToken
                ),
            cancellationToken
        );
    }

    private async ValueTask<IgnoreFile> FindIgnoreFileAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        var ignoreFile = await this.FindFileAsync(
            directoryName,
            this.ignoreFilesByDirectory,
            (searchingDirectory, cancellationToken) =>
                this.fileSystem.File.Exists(Path.Combine(searchingDirectory, ".gitignore"))
                || this.fileSystem.File.Exists(
                    Path.Combine(searchingDirectory, ".csharpierignore")
                ),
            (searchingDirectory, cancellationToken) =>
                IgnoreFile.CreateAsync(
                    searchingDirectory,
                    this.fileSystem,
                    null,
                    cancellationToken
                ),
            cancellationToken
        );

#pragma warning disable IDE0270
        if (ignoreFile is null)
        {
            // should never happen
            throw new Exception("Unable to locate an IgnoreFile for " + directoryName);
        }
#pragma warning restore IDE0270

        return ignoreFile;
    }

    /// <summary>
    /// this is a type of lazy lookup. We preload file type for the initial directory of the format command
    /// When trying to format a file in a given subdirectory if we've already found the appropriate file type then return it
    /// otherwise track it down (parsing if we need to) and set the references for any parent directories
    /// </summary>
    private async ValueTask<T?> FindFileAsync<T>(
        string directoryName,
        ConcurrentDictionary<string, T?> dictionary,
        Func<string, CancellationToken, bool> shouldConsiderDirectory,
        Func<string, CancellationToken, Task<T?>> createFileAsync,
        CancellationToken cancellationToken
    )
    {
        if (dictionary.TryGetValue(directoryName, out var result))
        {
            return result;
        }

        var directoriesToSet = new List<string>();
        var searchingDirectory = this.fileSystem.DirectoryInfo.New(directoryName);
        while (
            searchingDirectory is not null
            && !dictionary.TryGetValue(searchingDirectory.FullName, out result)
        )
        {
            if (shouldConsiderDirectory(searchingDirectory.FullName, cancellationToken))
            {
                dictionary[searchingDirectory.FullName] = result = await createFileAsync(
                    searchingDirectory.FullName,
                    cancellationToken
                );
                break;
            }

            directoriesToSet.Add(searchingDirectory.FullName);
            searchingDirectory = searchingDirectory.Parent;
        }

        foreach (var directoryToSet in directoriesToSet)
        {
            dictionary[directoryToSet] = result;
        }

        return result;
    }

    public async ValueTask<bool> IsIgnoredAsync(
        string actualFilePath,
        CancellationToken cancellationToken
    )
    {
        return (
            await this.FindIgnoreFileAsync(
                Path.GetDirectoryName(actualFilePath)!,
                cancellationToken
            )
        ).IsIgnored(actualFilePath);
    }

    public string Serialize()
    {
        return JsonSerializer.Serialize(
            new
            {
                specified = this.specifiedConfigFile,
                this.csharpierConfigsByDirectory,
                this.editorConfigByDirectory,
            }
        );
    }
}
