using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Cli.EditorConfig;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli.Options;

internal class OptionsProvider
{
    // TODO #1228 we are probably storing more than we need, if the directory doesn't contain an editorconfig, we can use the parent one
    private readonly Dictionary<string, EditorConfigSections?> editorConfigByDirectory = new();
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
            optionsProvider.editorConfigByDirectory[directoryName] = EditorConfigParser
                .FindForDirectoryName(
                    Path.GetDirectoryName(editorConfigPath)!,
                    fileSystem,
                    ignoreFile
                )
                .First();
        }
        // # TODO 1228 we should find the editorconfig here that DOES go up directories when searching


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

        if (!this.editorConfigByDirectory.TryGetValue(directoryName, out var resolvedEditorConfig))
        {
            this.editorConfigByDirectory[directoryName] = resolvedEditorConfig = EditorConfigParser
                .FindForDirectoryName(directoryName, this.fileSystem, this.ignoreFile)
                .FirstOrDefault();
        }

        // TODO #1228 the above probably does more parsing than it needs to, the below version
        // attempts to be smarter, but has issues. Maybe it isn't worth trying to get working
        // var searchingDirectory = new DirectoryInfo(directoryName);
        // EditorConfigSections? resolvedEditorConfig;
        // while (
        //     !this.editorConfigByDirectory.TryGetValue(
        //         searchingDirectory.FullName,
        //         out resolvedEditorConfig
        //     )
        // )
        // {
        //     if (
        //         this.fileSystem.File.Exists(
        //             Path.Combine(searchingDirectory.FullName, ".editorconfig")
        //         )
        //     )
        //     {
        //         this.editorConfigByDirectory[searchingDirectory.FullName] = resolvedEditorConfig =
        //             EditorConfigParser
        //                 .FindForDirectoryName(
        //                     searchingDirectory.FullName,
        //                     this.fileSystem,
        //                     this.ignoreFile
        //                 )
        //                 .FirstOrDefault();
        //         break;
        //     }
        //
        //     searchingDirectory = searchingDirectory.Parent;
        //     if (searchingDirectory is null)
        //     {
        //         break;
        //     }
        // }

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
