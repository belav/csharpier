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

        var searchingDirectory = this.fileSystem.DirectoryInfo.New(directoryName);
        CSharpierConfigData? resolvedCSharpierConfig;
        var directoriesToSet = new List<string>();
        while (
            !this.csharpierConfigsByDirectory.TryGetValue(
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
            if (searchingDirectory is null)
            {
                break;
            }
        }

        foreach (var directoryToSet in directoriesToSet)
        {
            this.csharpierConfigsByDirectory[directoryToSet] = resolvedCSharpierConfig;
        }

        if (resolvedCSharpierConfig is not null)
        {
            return resolvedCSharpierConfig.CSharpierConfig.ConvertToPrinterOptions(filePath);
        }

        searchingDirectory = this.fileSystem.DirectoryInfo.New(directoryName);
        EditorConfigSections? resolvedEditorConfig;
        directoriesToSet = new List<string>();
        while (
            !this.editorConfigByDirectory.TryGetValue(
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
            if (searchingDirectory is null)
            {
                break;
            }
        }

        foreach (var directoryToSet in directoriesToSet)
        {
            this.editorConfigByDirectory[directoryToSet] = resolvedEditorConfig;
        }

        if (resolvedEditorConfig is not null)
        {
            return resolvedEditorConfig.ConvertToPrinterOptions(filePath, false);
        }

        var formatter = PrinterOptions.GetFormatter(filePath);
        return formatter != Formatter.Unknown ? new PrinterOptions(formatter) : null;
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
