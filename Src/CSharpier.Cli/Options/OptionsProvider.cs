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
    private readonly List<CSharpierConfigData> csharpierConfigs;
    private readonly IgnoreFile ignoreFile;
    private readonly ConfigurationFileOptions? specifiedConfigFile;
    private readonly bool hasSpecificEditorConfig;
    private readonly IFileSystem fileSystem;

    private OptionsProvider(
        List<CSharpierConfigData> csharpierConfigs,
        IgnoreFile ignoreFile,
        ConfigurationFileOptions? specifiedPrinterOptions,
        bool hasSpecificEditorConfig,
        IFileSystem fileSystem
    )
    {
        this.csharpierConfigs = csharpierConfigs;
        this.ignoreFile = ignoreFile;
        this.specifiedConfigFile = specifiedPrinterOptions;
        this.hasSpecificEditorConfig = hasSpecificEditorConfig;
        this.fileSystem = fileSystem;
    }

    public static async Task<OptionsProvider> Create(
        string directoryName,
        string? configPath,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken,
        bool limitConfigSearch = false
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

        var csharpierConfigs = csharpierConfigPath is null
            ? CSharpierConfigParser.FindForDirectoryName(
                directoryName,
                fileSystem,
                logger,
                limitConfigSearch
            )
            : [];

        var ignoreFile = await IgnoreFile.Create(directoryName, fileSystem, cancellationToken);

        var optionsProvider = new OptionsProvider(
            csharpierConfigs,
            ignoreFile,
            specifiedConfigFile,
            hasSpecificEditorConfig: editorConfigPath is not null,
            fileSystem
        );

        if (editorConfigPath is not null)
        {
            optionsProvider.editorConfigByDirectory[directoryName] =
                EditorConfigLocator.FindForDirectoryName(
                    Path.GetDirectoryName(editorConfigPath)!,
                    fileSystem,
                    ignoreFile
                );
        }
        else
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

        if (this.hasSpecificEditorConfig)
        {
            return this
                .editorConfigByDirectory.Values.First()!
                .ConvertToPrinterOptions(filePath, true);
        }

        var directoryName = this.fileSystem.Path.GetDirectoryName(filePath);

        ArgumentNullException.ThrowIfNull(directoryName);

        // if (!this.editorConfigByDirectory.TryGetValue(directoryName, out var resolvedEditorConfig))
        // {
        //     DebugLogger.Log("Missing EditorConfig entry for " + directoryName);
        //     this.editorConfigByDirectory[directoryName] = resolvedEditorConfig = EditorConfigLocator
        //         .FindForDirectoryName(directoryName, this.fileSystem, this.ignoreFile)
        //         .FirstOrDefault();
        // }

        var searchingDirectory = new DirectoryInfo(directoryName);
        EditorConfigSections? resolvedEditorConfig;
        var directoriesToSet = new List<string>();
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

        var resolvedCSharpierConfig = this.csharpierConfigs.FirstOrDefault(o =>
            directoryName.StartsWith(o.DirectoryName, StringComparison.Ordinal)
        );

        if (resolvedCSharpierConfig is not null)
        {
            return resolvedCSharpierConfig.CSharpierConfig.ConvertToPrinterOptions(filePath);
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
                this.csharpierConfigs,
                this.editorConfigByDirectory,
            }
        );
    }
}
