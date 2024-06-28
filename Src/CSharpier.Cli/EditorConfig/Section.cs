namespace CSharpier.Cli.EditorConfig;

using IniParser.Model;

internal class Section
{
    private readonly GlobMatcher matcher;

    public string? IndentStyle { get; }
    public string? IndentSize { get; }
    public string? TabWidth { get; }
    public string? MaxLineLength { get; }
    public string? EndOfLine { get; }

    public Section(SectionData section, string directory)
    {
        this.matcher = Globber.Create(section.SectionName, directory);
        this.IndentStyle = section.Keys["indent_style"];
        this.IndentSize = section.Keys["indent_size"];
        this.TabWidth = section.Keys["tab_width"];
        this.MaxLineLength = section.Keys["max_line_length"];
        this.EndOfLine = section.Keys["end_of_line"];
    }

    public bool IsMatch(string fileName)
    {
        return this.matcher.IsMatch(fileName);
    }
}
