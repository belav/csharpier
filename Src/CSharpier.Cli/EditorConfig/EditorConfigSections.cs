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

        if (resolvedConfiguration.NewLineBeforeOpenBrace is { } newLineBeforeOpenBrace)
        {
            printerOptions.NewLineBeforeOpenBrace = newLineBeforeOpenBrace;
        }

        printerOptions.NewLineBeforeElse = resolvedConfiguration.NewLineBeforeElse ?? printerOptions.NewLineBeforeElse;
        printerOptions.NewLineBeforeCatch = resolvedConfiguration.NewLineBeforeCatch ?? printerOptions.NewLineBeforeCatch;
        printerOptions.NewLineBeforeFinally = resolvedConfiguration.NewLineBeforeFinally ?? printerOptions.NewLineBeforeFinally;
        printerOptions.NewLineBeforeMembersInObjectInitializers = resolvedConfiguration.NewLineBeforeMembersInObjectInitializers ?? printerOptions.NewLineBeforeMembersInObjectInitializers;
        printerOptions.NewLineBeforeMembersInAnonymousTypes = resolvedConfiguration.NewLineBeforeMembersInAnonymousTypes ?? printerOptions.NewLineBeforeMembersInAnonymousTypes;
        printerOptions.NewLineBetweenQueryExpressionClauses = resolvedConfiguration.NewLineBetweenQueryExpressionClauses ?? printerOptions.NewLineBetweenQueryExpressionClauses;

        return printerOptions;
    }

    private class ResolvedConfiguration
    {
        public string? IndentStyle { get; }
        public int? IndentSize { get; }
        public int? TabWidth { get; }
        public int? MaxLineLength { get; }
        public EndOfLine? EndOfLine { get; }

        public BraceNewLine? NewLineBeforeOpenBrace { get; }
        public bool? NewLineBeforeElse { get; }
        public bool? NewLineBeforeCatch { get; }
        public bool? NewLineBeforeFinally { get; }
        public bool? NewLineBeforeMembersInObjectInitializers { get; }
        public bool? NewLineBeforeMembersInAnonymousTypes { get; }
        public bool? NewLineBetweenQueryExpressionClauses { get; }

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

            var newLineBeforeOpenBrace = sections.LastOrDefault(o => o.NewLineBeforeOpenBrace != null)?.NewLineBeforeOpenBrace;
            if (!String.IsNullOrWhiteSpace(newLineBeforeOpenBrace))
            {
                this.NewLineBeforeOpenBrace = ConvertToBraceNewLine(newLineBeforeOpenBrace);
            }

            var newLineBeforeElse = sections.LastOrDefault(o => o.NewLineBeforeElse != null)?.NewLineBeforeElse;
            if (bool.TryParse(newLineBeforeElse, out var newLineBeforeElseValue))
            {
                this.NewLineBeforeElse = newLineBeforeElseValue;
            }

            var newLineBeforeCatch = sections.LastOrDefault(o => o.NewLineBeforeCatch != null)?.NewLineBeforeCatch;
            if (bool.TryParse(newLineBeforeCatch, out var newLineBeforeCatchValue))
            {
                this.NewLineBeforeCatch = newLineBeforeCatchValue;
            }

            var newLineBeforeFinally = sections.LastOrDefault(o => o.NewLineBeforeFinally != null)?.NewLineBeforeFinally;
            if (bool.TryParse(newLineBeforeFinally, out var newLineBeforeFinallyValue))
            {
                this.NewLineBeforeFinally = newLineBeforeFinallyValue;
            }

            var newLineBeforeMembersInObjectInitializers = sections.LastOrDefault(o => o.NewLineBeforeMembersInObjectInitializers != null)?.NewLineBeforeMembersInObjectInitializers;
            if (!String.IsNullOrWhiteSpace(newLineBeforeMembersInObjectInitializers) && bool.TryParse(newLineBeforeMembersInObjectInitializers, out var newLineBeforeMembersInObjectInitializersValue))
            {
                this.NewLineBeforeMembersInObjectInitializers = newLineBeforeMembersInObjectInitializersValue;
            }

            var newLineBeforeMembersInAnonymousTypes = sections.LastOrDefault(o => o.NewLineBeforeMembersInAnonymousTypes != null)?.NewLineBeforeMembersInAnonymousTypes;
            if (!String.IsNullOrWhiteSpace(newLineBeforeMembersInAnonymousTypes) && bool.TryParse(newLineBeforeMembersInAnonymousTypes, out var newLineBeforeMembersInAnonymousTypesValue))
            {
                this.NewLineBeforeMembersInAnonymousTypes = newLineBeforeMembersInAnonymousTypesValue;
            }

            var newLineBetweenQueryExpressionClauses = sections.LastOrDefault(o => o.NewLineBetweenQueryExpressionClauses != null)?.NewLineBetweenQueryExpressionClauses;
            if (!String.IsNullOrWhiteSpace(newLineBetweenQueryExpressionClauses) && bool.TryParse(newLineBetweenQueryExpressionClauses, out var newLineBetweenQueryExpressionClausesValue))
            {
                this.NewLineBetweenQueryExpressionClauses = newLineBetweenQueryExpressionClausesValue;
            }
        }
    }

    public static BraceNewLine ConvertToBraceNewLine(string input)
    {
        // Convert the string to a list of enum values
        BraceNewLine result = BraceNewLine.None;
        string[] values = input.Split(',');

        foreach (var value in values)
        {
            // Normalize the string to match the enum naming
            string enumValueName = value.Replace("_", "").ToLower();
            foreach (BraceNewLine enumValue in Enum.GetValues(typeof(BraceNewLine)))
            {
                if (enumValue.ToString().ToLower() == enumValueName)
                {
                    result |= enumValue;
                    break;
                }
            }
        }

        return result;
    }
}
