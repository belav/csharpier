using IniParser.Model;

namespace CSharpier.Cli.EditorConfig;

internal class Section(SectionData section, string directory)
{
    private readonly GlobMatcher matcher = Globber.Create(section.SectionName, directory);
    private readonly GlobMatcher noDirectoryMatcher = Globber.Create(section.SectionName, null);

    public string? IndentStyle { get; } = section.Keys["indent_style"];
    public string? IndentSize { get; } = section.Keys["indent_size"];
    public string? TabWidth { get; } = section.Keys["tab_width"];
    public string? MaxLineLength { get; } = section.Keys["max_line_length"];
    public string? EndOfLine { get; } = section.Keys["end_of_line"];
    public string? Formatter { get; } = section.Keys["csharpier_formatter"];

    public bool IsMatch(string fileName, bool ignoreDirectory)
    {
        return ignoreDirectory
            ? this.noDirectoryMatcher.IsMatch(fileName)
            : this.matcher.IsMatch(fileName);
    }
}
