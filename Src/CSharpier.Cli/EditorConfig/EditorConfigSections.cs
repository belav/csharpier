namespace CSharpier.Cli.EditorConfig;

/// <summary>
/// This is a representation of the editorconfig for the given directory along with
/// sections from any parent files until a root file is found
/// </summary>
internal class EditorConfigSections
{
    public required string DirectoryName { get; init; }
    public required IReadOnlyCollection<Section> SectionsIncludingParentFiles { get; init; }

    public PrinterOptions ConvertToPrinterOptions(string filePath)
    {
        var sections = this.SectionsIncludingParentFiles.Where(o => o.IsMatch(filePath)).ToList();
        var resolvedConfiguration = new ResolvedConfiguration(sections);
        var printerOptions = new PrinterOptions();

        if (resolvedConfiguration.MaxLineLength is { } maxLineLength)
        {
            printerOptions.Width = maxLineLength;
        }

        if (resolvedConfiguration.IndentStyle is "tab")
        {
            printerOptions.UseTabs = true;
        }

        if (printerOptions.UseTabs)
        {
            printerOptions.TabWidth = resolvedConfiguration.TabWidth ?? printerOptions.TabWidth;
        }
        else
        {
            printerOptions.TabWidth = resolvedConfiguration.IndentSize ?? printerOptions.TabWidth;
        }

        if (resolvedConfiguration.EndOfLine is { } endOfLine)
        {
            printerOptions.EndOfLine = endOfLine;
        }

        return printerOptions;
    }

    private class ResolvedConfiguration
    {
        public string? IndentStyle { get; }
        public int? IndentSize { get; }
        public int? TabWidth { get; }
        public int? MaxLineLength { get; }
        public EndOfLine? EndOfLine { get; }

        public ResolvedConfiguration(List<Section> sections)
        {
            var indentStyle = sections.LastOrDefault(o => o.IndentStyle != null)?.IndentStyle;
            if (indentStyle is "space" or "tab")
            {
                this.IndentStyle = indentStyle;
            }

            var maxLineLength = sections.LastOrDefault(o => o.MaxLineLength != null)?.MaxLineLength;
            if (int.TryParse(maxLineLength, out var maxLineLengthValue) && maxLineLengthValue > 0)
            {
                this.MaxLineLength = maxLineLengthValue;
            }

            var indentSize = sections.LastOrDefault(o => o.IndentSize != null)?.IndentSize;
            var tabWidth = sections.LastOrDefault(o => o.TabWidth != null)?.TabWidth;

            if (indentSize == "tab")
            {
                if (int.TryParse(tabWidth, out var tabWidthValue))
                {
                    this.TabWidth = tabWidthValue;
                }

                this.IndentSize = this.TabWidth;
            }
            else
            {
                if (int.TryParse(indentSize, out var indentSizeValue))
                {
                    this.IndentSize = indentSizeValue;
                }

                this.TabWidth = int.TryParse(tabWidth, out var tabWidthValue)
                    ? tabWidthValue
                    : this.IndentSize;
            }

            var endOfLine = sections.LastOrDefault(o => o.EndOfLine != null)?.EndOfLine;
            if (Enum.TryParse(endOfLine, true, out EndOfLine result))
            {
                this.EndOfLine = result;
            }
        }
    }
}
