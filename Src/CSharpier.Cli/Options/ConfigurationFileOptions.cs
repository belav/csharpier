namespace CSharpier.Cli.Options;

using System.Text.Json.Serialization;
using CSharpier.Cli.EditorConfig;

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
        var matchingOverride = this.Overrides.LastOrDefault(o => o.IsMatch(filePath));
        if (matchingOverride is not null)
        {
            return new PrinterOptions
            {
                TabWidth = matchingOverride.TabWidth,
                UseTabs = matchingOverride.UseTabs,
                Width = matchingOverride.PrintWidth,
                EndOfLine = matchingOverride.EndOfLine,
                Formatter = matchingOverride.Formatter
            };
        }

        if (filePath.EndsWith(".cs") || filePath.EndsWith(".csx"))
        {
            return new PrinterOptions
            {
                TabWidth = this.TabWidth,
                UseTabs = this.UseTabs,
                Width = this.PrintWidth,
                EndOfLine = this.EndOfLine,
                Formatter = "csharp"
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

    // TODO overrides what about specifying this in editorconfig?
    // TODO overrides need to format files with non-standard extensions
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
