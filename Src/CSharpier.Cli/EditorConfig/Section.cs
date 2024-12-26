using IniParser.Model;

namespace CSharpier.Cli.EditorConfig;

internal class Section(SectionData section, string directory)
{
    private readonly GlobMatcher matcher = Globber.Create(section.SectionName, directory);

    public string? IndentStyle { get; } = section.Keys["indent_style"];
    public string? IndentSize { get; } = section.Keys["indent_size"];
    public string? TabWidth { get; } = section.Keys["tab_width"];
    public string? MaxLineLength { get; } = section.Keys["max_line_length"];
    public string? EndOfLine { get; } = section.Keys["end_of_line"];
    public string? Formatter { get; } = section.Keys["csharpier_formatter"];

    public bool IsMatch(string fileName)
    {
        return this.matcher.IsMatch(fileName);
    }
}
