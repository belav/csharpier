using System.Text.Json.Serialization;
using CSharpier.Cli.EditorConfig;
using CSharpier.Core;

namespace CSharpier.Cli.Options;

internal class ConfigurationFileOptions
{
    public int? PrintWidth { get; init; }
    public int? IndentSize { get; init; }
    public bool UseTabs { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<XmlWhitespaceSensitivity>))]
    public XmlWhitespaceSensitivity? XmlWhitespaceSensitivity { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<EndOfLine>))]
    public EndOfLine EndOfLine { get; init; }

    public Override[] Overrides { get; init; } = [];

    public PrinterOptions? ConvertToPrinterOptions(string filePath)
    {
        DebugLogger.Log("finding options for " + filePath);
        var matchingOverride = this.Overrides.LastOrDefault(o => o.IsMatch(filePath));
        if (matchingOverride is not null)
        {
            if (
                !Enum.TryParse<Formatter>(
                    matchingOverride.Formatter,
                    ignoreCase: true,
                    out var parsedFormatter
                )
            )
            {
                return null;
            }

            return CreatePrinterOptions(
                parsedFormatter,
                filePath,
                matchingOverride.XmlWhitespaceSensitivity,
                matchingOverride.IndentSize,
                matchingOverride.PrintWidth,
                matchingOverride.UseTabs,
                matchingOverride.EndOfLine
            );
        }

        var formatter = PrinterOptions.GetFormatter(filePath);
        if (formatter != Formatter.Unknown)
        {
            return CreatePrinterOptions(
                formatter,
                filePath,
                this.XmlWhitespaceSensitivity,
                this.IndentSize,
                this.PrintWidth,
                this.UseTabs,
                this.EndOfLine
            );
        }

        return null;
    }

    private static PrinterOptions CreatePrinterOptions(
        Formatter formatter,
        string filePath,
        XmlWhitespaceSensitivity? xmlWhitespaceSensitivity,
        int? indentSize,
        int? printWidth,
        bool useTabs,
        EndOfLine endOfLine
    )
    {
        var printerOptions = new PrinterOptions(
            formatter,
            xmlWhitespaceSensitivity ?? PrinterOptions.GetXmlWhitespaceSensitivity(filePath)
        )
        {
            UseTabs = useTabs,
            EndOfLine = endOfLine,
        };

        if (indentSize is not null)
        {
            printerOptions.IndentSize = indentSize.Value;
        }

        if (printWidth is not null)
        {
            printerOptions.Width = printWidth.Value;
        }

        return printerOptions;
    }

    public void Init(string directory)
    {
        foreach (var thing in this.Overrides)
        {
            thing.Init(directory);
        }
    }
}

internal class Override
{
    private GlobMatcher? matcher;

    public int? PrintWidth { get; init; }
    public int? IndentSize { get; init; }
    public bool UseTabs { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<XmlWhitespaceSensitivity>))]
    public XmlWhitespaceSensitivity? XmlWhitespaceSensitivity { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<EndOfLine>))]
    public EndOfLine EndOfLine { get; init; }

    public string Files { get; init; } = string.Empty;

    public string Formatter { get; init; } = string.Empty;

    public void Init(string directory)
    {
        this.matcher = Globber.Create(this.Files, directory);
    }

    public bool IsMatch(string fileName)
    {
        return this.matcher?.IsMatch(fileName) ?? false;
    }
}
