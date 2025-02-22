using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Cli.EditorConfig;
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
    private readonly ConcurrentDictionary<string, IgnoreFile> ignoreFilesByDirectory = new();
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

    public static async Task<OptionsProvider> Create(
        string directoryName,
        string? configPath,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        while (!System.Diagnostics.Debugger.IsAttached)
        {
            Thread.Sleep(100);
        }

        DebugLogger.Log("Create " + directoryName);
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

        var ignoreFile = await IgnoreFile.CreateAsync(directoryName, fileSystem, cancellationToken);

        var specifiedEditorConfig = editorConfigPath is not null
            ? EditorConfigLocator.FindForDirectoryName(
                Path.GetDirectoryName(editorConfigPath)!,
                fileSystem,
                ignoreFile
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
                EditorConfigLocator.FindForDirectoryName(directoryName, fileSystem, ignoreFile);
        }

        return optionsProvider;
    }

    public async Task<PrinterOptions?> GetPrinterOptionsForAsync(
        string filePath,
        CancellationToken cancellationToken
    )
    {
        DebugLogger.Log("GetPrinterOptionsForAsync " + filePath);
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

        var resolvedCSharpierConfig = this.FindCSharpierConfig(directoryName);
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

    /// <summary>
    /// this is a type of lazy lookup. We preload a csharpierconfig for the initial directory of the format command
    /// For a file in a given subdirectory if we've already found the appropriate cshapierconfig return it
    /// otherwise track it down (parsing if we need to) and set the references for any parent directories
    /// </summary>
    private CSharpierConfigData? FindCSharpierConfig(string directoryName)
    {
        if (
            this.csharpierConfigsByDirectory.TryGetValue(
                directoryName,
                out var resolvedCSharpierConfig
            )
        )
        {
            return resolvedCSharpierConfig;
        }

        var searchingDirectory = this.fileSystem.DirectoryInfo.New(directoryName);
        var directoriesToSet = new List<string>();
        while (
            searchingDirectory is not null
            && !this.csharpierConfigsByDirectory.TryGetValue(
                searchingDirectory.FullName,
                out resolvedCSharpierConfig
            )
        )
        {
            if (
                searchingDirectory
                    .EnumerateFiles(".csharpierrc*", SearchOption.TopDirectoryOnly)
                    .Any()
            )
            {
                this.csharpierConfigsByDirectory[searchingDirectory.FullName] =
                    resolvedCSharpierConfig = CSharpierConfigParser.FindForDirectoryName(
                        directoryName,
                        this.fileSystem,
                        this.logger
                    );
                break;
            }

            directoriesToSet.Add(searchingDirectory.FullName);
            searchingDirectory = searchingDirectory.Parent;
        }

        foreach (var directoryToSet in directoriesToSet)
        {
            this.csharpierConfigsByDirectory[directoryToSet] = resolvedCSharpierConfig;
        }

        return resolvedCSharpierConfig;
    }

    private async Task<EditorConfigSections?> FindEditorConfigAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        if (this.editorConfigByDirectory.TryGetValue(directoryName, out var resolvedEditorConfig))
        {
            return resolvedEditorConfig;
        }

        var directoriesToSet = new List<string>();
        var searchingDirectory = this.fileSystem.DirectoryInfo.New(directoryName);
        while (
            searchingDirectory is not null
            && !this.editorConfigByDirectory.TryGetValue(
                searchingDirectory.FullName,
                out resolvedEditorConfig
            )
        )
        {
            if (
                this.fileSystem.File.Exists(
                    Path.Combine(searchingDirectory.FullName, ".editorconfig")
                )
            )
            {
                this.editorConfigByDirectory[searchingDirectory.FullName] = resolvedEditorConfig =
                    EditorConfigLocator.FindForDirectoryName(
                        searchingDirectory.FullName,
                        this.fileSystem,
                        await this.FindIgnoreFileAsync(
                            searchingDirectory.FullName,
                            cancellationToken
                        )
                    );
                break;
            }

            directoriesToSet.Add(searchingDirectory.FullName);
            searchingDirectory = searchingDirectory.Parent;
        }

        foreach (var directoryToSet in directoriesToSet)
        {
            this.editorConfigByDirectory[directoryToSet] = resolvedEditorConfig;
        }

        return resolvedEditorConfig;
    }

    private async Task<IgnoreFile> FindIgnoreFileAsync(
        string directoryName,
        CancellationToken cancellationToken
    )
    {
        DebugLogger.Log("FindIgnoreFileAsync " + directoryName);
        if (this.ignoreFilesByDirectory.TryGetValue(directoryName, out var ignoreFile))
        {
            return ignoreFile;
        }

        var directoriesToSet = new List<string>();
        var searchingDirectory = this.fileSystem.DirectoryInfo.New(directoryName);
        while (
            searchingDirectory is not null
            && !this.ignoreFilesByDirectory.TryGetValue(searchingDirectory.FullName, out ignoreFile)
        )
        {
            if (
                this.fileSystem.File.Exists(Path.Combine(searchingDirectory.FullName, ".gitignore"))
                || this.fileSystem.File.Exists(
                    Path.Combine(searchingDirectory.FullName, ".csharpierignore")
                )
            )
            {
                this.ignoreFilesByDirectory[searchingDirectory.FullName] = ignoreFile =
                    await IgnoreFile.CreateAsync(
                        searchingDirectory.FullName,
                        this.fileSystem,
                        cancellationToken
                    );
                break;
            }

            directoriesToSet.Add(searchingDirectory.FullName);
            searchingDirectory = searchingDirectory.Parent;
        }

        if (ignoreFile is null)
        {
            // should never happen
            throw new Exception("Unable to locate an IgnoreFile for " + directoryName);
        }

        foreach (var directoryToSet in directoriesToSet)
        {
            this.ignoreFilesByDirectory[directoryToSet] = ignoreFile;
        }

        return ignoreFile;
    }

    public async Task<bool> IsIgnoredAsync(
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
