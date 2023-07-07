namespace CSharpier.Cli.Options;

using System.IO.Abstractions;
using CSharpier.Cli.EditorConfig;
using Microsoft.Extensions.Logging;
using PrinterOptions = CSharpier.PrinterOptions;

internal class OptionsProvider
{
    private readonly List<EditorConfigSections> configs;
    private readonly List<CSharpierConfigData> csharpierConfigs;
    private readonly IgnoreFile ignoreFile;
    private readonly PrinterOptions? specifiedPrinterOptions;
    private readonly IFileSystem fileSystem;

    private OptionsProvider(
        List<EditorConfigSections> configs,
        List<CSharpierConfigData> csharpierConfigs,
        IgnoreFile ignoreFile,
        PrinterOptions? specifiedPrinterOptions,
        IFileSystem fileSystem
    )
    {
        this.configs = configs;
        this.csharpierConfigs = csharpierConfigs;
        this.ignoreFile = ignoreFile;
        this.specifiedPrinterOptions = specifiedPrinterOptions;
        this.fileSystem = fileSystem;
    }

    public static async Task<OptionsProvider> Create(
        string directoryName,
        string? configPath,
        IFileSystem fileSystem,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        var specifiedPrinterOptions = configPath is not null
            ? ConfigurationFileOptions.CreatePrinterOptionsFromPath(configPath, fileSystem, logger)
            : null;

        var csharpierConfigs = configPath is null
            ? ConfigurationFileOptions.FindForDirectory2(directoryName, fileSystem, logger)
            : Array.Empty<CSharpierConfigData>().ToList();

        var editorConfigSections = EditorConfigParser.GetAllForDirectory(directoryName, fileSystem);
        var ignoreFile = await IgnoreFile.Create(directoryName, fileSystem, cancellationToken);

        return new OptionsProvider(
            editorConfigSections,
            csharpierConfigs,
            ignoreFile,
            specifiedPrinterOptions,
            fileSystem
        );
    }

    public PrinterOptions GetPrinterOptionsFor(string filePath)
    {
        if (this.specifiedPrinterOptions is not null)
        {
            return this.specifiedPrinterOptions;
        }

        var directoryName = this.fileSystem.Path.GetDirectoryName(filePath);
        var resolvedEditorConfig = this.configs.FirstOrDefault(
            o => o.DirectoryName.StartsWith(directoryName)
        );
        var resolvedCSharpierConfig = this.csharpierConfigs.FirstOrDefault(
            o => o.DirectoryName.StartsWith(directoryName)
        );

        if (resolvedEditorConfig is null && resolvedCSharpierConfig is null)
        {
            return new PrinterOptions();
        }

        if (
            (resolvedCSharpierConfig?.DirectoryName.Length ?? int.MaxValue)
            < (resolvedEditorConfig?.DirectoryName.Length ?? int.MaxValue)
        )
        {
            return ConfigurationFileOptions.ConvertToPrinterOptions(
                resolvedCSharpierConfig!.CSharpierConfig
            );
        }

        return resolvedEditorConfig!.ConvertToPrinterOptions(filePath);
    }

    public bool IsIgnored(string actualFilePath)
    {
        return this.ignoreFile.IsIgnored(actualFilePath);
    }
}
