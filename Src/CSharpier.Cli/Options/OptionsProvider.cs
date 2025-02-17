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
    private readonly IgnoreFile ignoreFile;
    private readonly ConfigurationFileOptions? specifiedConfigFile;
    private readonly EditorConfigSections? specifiedEditorConfig;
    private readonly IFileSystem fileSystem;
    private readonly ILogger logger;

    private OptionsProvider(
        IgnoreFile ignoreFile,
        ConfigurationFileOptions? specifiedPrinterOptions,
        EditorConfigSections? specifiedEditorConfig,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        this.ignoreFile = ignoreFile;
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

        var ignoreFile = await IgnoreFile.Create(directoryName, fileSystem, cancellationToken);

        var specifiedEditorConfig = editorConfigPath is not null
            ? EditorConfigLocator.FindForDirectoryName(
                Path.GetDirectoryName(editorConfigPath)!,
                fileSystem,
                ignoreFile
            )
            : null;

        var optionsProvider = new OptionsProvider(
            ignoreFile,
            specifiedConfigFile,
            specifiedEditorConfig,
            fileSystem,
            logger
        );

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

    public PrinterOptions? GetPrinterOptionsFor(string filePath)
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

        var resolvedCSharpierConfig = this.FindCSharpierConfig(directoryName);
        if (resolvedCSharpierConfig is not null)
        {
            return resolvedCSharpierConfig.CSharpierConfig.ConvertToPrinterOptions(filePath);
        }

        var resolvedEditorConfig = this.FindEditorConfig(directoryName);
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

    private EditorConfigSections? FindEditorConfig(string directoryName)
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
                        this.ignoreFile
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

    public bool IsIgnored(string actualFilePath)
    {
        return this.ignoreFile.IsIgnored(actualFilePath);
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
