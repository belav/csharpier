using System.Text.Json.Serialization;
using CSharpier.Cli.EditorConfig;

namespace CSharpier.Cli.Options;

internal class ConfigurationFileOptions
{
    public int PrintWidth { get; init; } = 100;
    public int TabWidth { get; init; } = 4;
    public bool UseTabs { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<EndOfLine>))]
    public EndOfLine EndOfLine { get; init; }

    public Override[] Overrides { get; init; } = [];

    public PrinterOptions? ConvertToPrinterOptions(string filePath)
    {
        DebugLogger.Log("finding options for " + filePath);
        var matchingOverride = this.Overrides.LastOrDefault(o => o.IsMatch(filePath));
        if (matchingOverride is not null)
        {
            return new PrinterOptions
            {
                IndentSize = matchingOverride.TabWidth,
                UseTabs = matchingOverride.UseTabs,
                Width = matchingOverride.PrintWidth,
                EndOfLine = matchingOverride.EndOfLine,
                Formatter = matchingOverride.Formatter,
            };
        }

        if (
            filePath.EndsWith(".cs", StringComparison.Ordinal)
            || filePath.EndsWith(".csx", StringComparison.Ordinal)
        )
        {
            return new PrinterOptions
            {
                IndentSize = this.TabWidth,
                UseTabs = this.UseTabs,
                Width = this.PrintWidth,
                EndOfLine = this.EndOfLine,
                Formatter = "csharp",
            };
        }

        return null;
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

    public int PrintWidth { get; init; } = 100;
    public int TabWidth { get; init; } = 4;
    public bool UseTabs { get; init; }

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
