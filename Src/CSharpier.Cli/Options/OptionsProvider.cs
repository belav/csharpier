namespace CSharpier.Cli.Options;

using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Cli.EditorConfig;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using PrinterOptions = CSharpier.PrinterOptions;

internal class OptionsProvider
{
    private readonly IList<EditorConfigSections> editorConfigs;
    private readonly List<CSharpierConfigData> csharpierConfigs;
    private readonly IgnoreFile ignoreFile;
    private readonly PrinterOptions? specifiedPrinterOptions;
    private readonly IFileSystem fileSystem;

    private OptionsProvider(
        IList<EditorConfigSections> editorConfigs,
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
        CancellationToken cancellationToken,
        bool limitEditorConfigSearch = false
    )
    {
        var specifiedPrinterOptions = configPath is not null
            ? ConfigurationFileOptions.CreatePrinterOptionsFromPath(configPath, fileSystem, logger)
            : null;

        var csharpierConfigs = configPath is null
            ? ConfigurationFileOptions.FindForDirectoryName(directoryName, fileSystem, logger)
            : Array.Empty<CSharpierConfigData>().ToList();

        IList<EditorConfigSections>? editorConfigSections = null;

        var ignoreFile = await IgnoreFile.Create(directoryName, fileSystem, cancellationToken);

        try
        {
            editorConfigSections = EditorConfigParser.FindForDirectoryName(
                directoryName,
                fileSystem,
                limitEditorConfigSearch,
                ignoreFile
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failure parsing editorconfig files for {DirectoryName}",
                directoryName
            );
        }

        return new OptionsProvider(
            editorConfigSections ?? Array.Empty<EditorConfigSections>(),
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

        if (resolvedCSharpierConfig is not null)
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
