using System.Collections.Concurrent;
using System.IO.Abstractions;
using CSharpier.Cli.DotIgnore;
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
    private readonly ConcurrentDictionary<string, IgnoreList> ignoreWithPathCache = new();
    private readonly ConcurrentDictionary<string, IgnoreFile?> ignoreFilesByDirectory = new();
    private readonly ConfigurationFileOptions? specifiedConfigFile;
    private readonly EditorConfigSections? specifiedEditorConfig;
    private readonly bool hasSpecifiedIgnorePath;
    private readonly IFileSystem fileSystem;
    private readonly ILogger logger;

    private OptionsProvider(
        ConfigurationFileOptions? specifiedPrinterOptions,
        EditorConfigSections? specifiedEditorConfig,
        bool hasSpecifiedIgnorePath,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        this.specifiedConfigFile = specifiedPrinterOptions;
        this.specifiedEditorConfig = specifiedEditorConfig;
        this.hasSpecifiedIgnorePath = hasSpecifiedIgnorePath;
        this.fileSystem = fileSystem;
        this.logger = logger;
    }

    public static async Task<OptionsProvider> Create(
        string directoryName,
        string? configPath,
        string? ignorePath,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        return await Create(
            [directoryName],
            configPath,
            ignorePath,
            fileSystem,
            logger,
            cancellationToken
        );
    }

    public static async Task<OptionsProvider> Create(
        IEnumerable<string> directoryNames,
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

        var specifiedEditorConfig = editorConfigPath is not null
            ? await EditorConfigLocator.FindForDirectoryNameAsync(
                Path.GetDirectoryName(editorConfigPath)!,
                fileSystem,
                cancellationToken
            )
            : null;

        var optionsProvider = new OptionsProvider(
            specifiedConfigFile,
            specifiedEditorConfig,
            ignorePath is not null,
            fileSystem,
            logger
        );

        var distinctDirectoryNames = directoryNames.Distinct(StringComparer.Ordinal).ToArray();
        foreach (var directoryName in distinctDirectoryNames)
        {
            var ignoreFile = await IgnoreFile.CreateAsync(
                directoryName,
                fileSystem,
                ignorePath,
                null,
                cancellationToken
            );
            optionsProvider.ignoreFilesByDirectory[directoryName] =
                ignoreFile ?? IgnoreFile.NullIgnore;
        }

        if (distinctDirectoryNames.Length == 1)
        {
            var firstDirectoryName = distinctDirectoryNames[0];
            if (csharpierConfigPath is null)
            {
                optionsProvider.csharpierConfigsByDirectory[firstDirectoryName] =
                    CSharpierConfigParser.FindForDirectoryName(
                        firstDirectoryName,
                        fileSystem,
                        logger
                    );
            }

            if (editorConfigPath is null)
            {
                optionsProvider.editorConfigByDirectory[firstDirectoryName] =
                    await EditorConfigLocator.FindForDirectoryNameAsync(
                        firstDirectoryName,
                        fileSystem,
                        cancellationToken
                    );
            }
        }

        return optionsProvider;
    }

    public async Task<PrinterOptions?> GetPrinterOptionsForAsync(
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
        return formatter != Formatter.Unknown
            ? new PrinterOptions(formatter, PrinterOptions.GetXmlWhitespaceSensitivity(filePath))
            : null;
    }

    private Task<CSharpierConfigData?> FindCSharpierConfigAsync(string directoryName)
    {
        return this.FindFileAsync(
            directoryName,
            this.csharpierConfigsByDirectory,
            searchingDirectory =>
                this.fileSystem.Directory.EnumerateFiles(
                        searchingDirectory,
                        ".csharpierrc*",
                        SearchOption.TopDirectoryOnly
                    )
                    .Any(),
            searchingDirectory =>
                Task.FromResult(
                    CSharpierConfigParser.FindForDirectoryName(
                        searchingDirectory,
                        this.fileSystem,
                        this.logger
                    )
                )
        );
    }

    private async Task<EditorConfigSections?> FindEditorConfigAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        return await this.FindFileAsync(
            directoryName,
            this.editorConfigByDirectory,
            searchingDirectory =>
                this.fileSystem.File.Exists(Path.Combine(searchingDirectory, ".editorconfig")),
            searchingDirectory =>
                EditorConfigLocator.FindForDirectoryNameAsync(
                    searchingDirectory,
                    this.fileSystem,
                    cancellationToken
                )
        );
    }

    private async Task<IgnoreFile> FindIgnoreFileAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        var ignoreFile = await this.FindFileAsync(
            directoryName,
            this.ignoreFilesByDirectory,
            (searchingDirectory) =>
                this.fileSystem.File.Exists(Path.Combine(searchingDirectory, ".gitignore"))
                || this.fileSystem.File.Exists(
                    Path.Combine(searchingDirectory, ".csharpierignore")
                ),
            (searchingDirectory) =>
                IgnoreFile.CreateAsync(
                    searchingDirectory,
                    this.fileSystem,
                    null,
                    ignoreWithPathCache,
                    cancellationToken
                )
        );

        return ignoreFile ?? IgnoreFile.NullIgnore;
    }

    /// <summary>
    /// this is a type of lazy lookup. We preload file type for the initial directory of the format command
    /// When trying to format a file in a given subdirectory if we've already found the appropriate file type then return it
    /// otherwise track it down (parsing if we need to) and set the references for any parent directories
    /// </summary>
    private async Task<T?> FindFileAsync<T>(
        string directoryName,
        ConcurrentDictionary<string, T?> dictionary,
        Func<string, bool> shouldConsiderDirectory,
        Func<string, Task<T?>> createFileAsync
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
            if (
                this.fileSystem.Directory.Exists(searchingDirectory.FullName)
                && shouldConsiderDirectory(searchingDirectory.FullName)
            )
            {
                dictionary[searchingDirectory.FullName] = result = await createFileAsync(
                    searchingDirectory.FullName
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

    public Task<bool> IsFileIgnoredAsync(
        string filePath,
        string? ignoreRootDirectory,
        CancellationToken cancellationToken
    )
    {
        return this.IsIgnoredAsync(filePath, ignoreRootDirectory, false, cancellationToken);
    }

    public Task<bool> IsFileIgnoredAsync(string filePath, CancellationToken cancellationToken)
    {
        return this.IsFileIgnoredAsync(filePath, null, cancellationToken);
    }

    public Task<bool> IsDirectoryIgnoredAsync(
        string filePath,
        string ignoreRootDirectory,
        CancellationToken cancellationToken
    )
    {
        return this.IsIgnoredAsync(filePath, ignoreRootDirectory, true, cancellationToken);
    }

    private async Task<bool> IsIgnoredAsync(
        string path,
        string? ignoreRootDirectory,
        bool isDirectory,
        CancellationToken cancellationToken
    )
    {
        if (
            this.hasSpecifiedIgnorePath
            && ignoreRootDirectory is not null
            && this.ignoreFilesByDirectory.TryGetValue(
                ignoreRootDirectory,
                out var specifiedIgnoreFile
            )
            && specifiedIgnoreFile is not null
        )
        {
            return specifiedIgnoreFile.IsIgnored(path, isDirectory);
        }

        return (
            await this.FindIgnoreFileAsync(Path.GetDirectoryName(path)!, cancellationToken)
        ).IsIgnored(path, isDirectory);
    }
}
