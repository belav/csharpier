namespace CSharpier.Cli.Options;

using System.IO.Abstractions;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

internal class ConfigurationFileOptions
{
    public int PrintWidth { get; init; } = 100;
    public int TabWidth { get; init; } = 4;
    public bool UseTabs { get; init; }

    public Override[] Overrides { get; init; } = [];

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<EndOfLine>))]
    public EndOfLine EndOfLine { get; init; }

    internal static PrinterOptions CreatePrinterOptionsFromPath(
        string configPath,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        var configurationFileOptions = ConfigurationFileParser.Create(
            configPath,
            fileSystem,
            logger
        );

        return ConvertToPrinterOptions(configurationFileOptions);
    }

    internal static PrinterOptions ConvertToPrinterOptions(
        ConfigurationFileOptions configurationFileOptions
    )
    {
        return new PrinterOptions
        {
            TabWidth = configurationFileOptions.TabWidth,
            UseTabs = configurationFileOptions.UseTabs,
            Width = configurationFileOptions.PrintWidth,
            EndOfLine = configurationFileOptions.EndOfLine
        };
    }
}

// TODO actually make use of all of this
internal class Override
{
    // TODO figure out dealing with .cst vs cst
    public string[] Extensions { get; init; } = [];

    // TODO what about specifying this in editorconfig?
    // TODO need to format files with non-standard extensions
    public string Formatter { get; init; } = string.Empty;
    public int PrintWidth { get; init; } = 100;
    public int TabWidth { get; init; } = 4;
    public bool UseTabs { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<EndOfLine>))]
    public EndOfLine EndOfLine { get; init; }
}
