namespace CSharpier.Cli.Options;

using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Cli.EditorConfig;
using Microsoft.Extensions.Logging;
using PrinterOptions = CSharpier.PrinterOptions;

internal class OptionsProvider
{
    private readonly List<EditorConfigSections> editorConfigs;
    private readonly List<CSharpierConfigData> csharpierConfigs;
    private readonly IgnoreFile ignoreFile;
    private readonly PrinterOptions? specifiedPrinterOptions;
    private readonly IFileSystem fileSystem;

    private OptionsProvider(
        List<EditorConfigSections> editorConfigs,
        List<CSharpierConfigData> csharpierConfigs,
        IgnoreFile ignoreFile,
        PrinterOptions? specifiedPrinterOptions,
        IFileSystem fileSystem
    )
    {
        this.editorConfigs = editorConfigs;
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
            ? ConfigurationFileOptions.FindForDirectoryName(directoryName, fileSystem, logger)
            : Array.Empty<CSharpierConfigData>().ToList();

        var editorConfigSections = EditorConfigParser.FindForDirectoryName(
            directoryName,
            fileSystem
        );
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
        var resolvedEditorConfig = this.editorConfigs.FirstOrDefault(
            o => directoryName.StartsWith(o.DirectoryName)
        );
        var resolvedCSharpierConfig = this.csharpierConfigs.FirstOrDefault(
            o => directoryName.StartsWith(o.DirectoryName)
        );

        if (resolvedEditorConfig is null && resolvedCSharpierConfig is null)
        {
            return new PrinterOptions();
        }

        if (
            (resolvedCSharpierConfig?.DirectoryName.Length ?? int.MinValue)
            >= (resolvedEditorConfig?.DirectoryName.Length ?? int.MinValue)
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

    public string Serialize()
    {
        return JsonSerializer.Serialize(
            new
            {
                specified = this.specifiedPrinterOptions,
                csharpierConfigs = this.csharpierConfigs,
                editorConfigs = this.editorConfigs
            }
        );
    }
}
