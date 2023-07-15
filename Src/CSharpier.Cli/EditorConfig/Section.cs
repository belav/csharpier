namespace CSharpier.Cli.EditorConfig;

using DotNet.Globbing;

public class Section
{
    private readonly Glob matcher;
    public string Pattern { get; }
    public string? IndentStyle { get; }
    public string? IndentSize { get; }
    public string? TabWidth { get; }
    public string? MaxLineLength { get; }

    public Section(string name, string directory, Dictionary<string, string?> properties)
    {
        this.Pattern = FixGlob(name, directory);
        this.matcher = Glob.Parse(this.Pattern);
        this.IndentStyle = properties.TryGetValue("indent_style", out var indentStyle)
            ? indentStyle
            : null;
        this.IndentSize = properties.TryGetValue("indent_size", out var indentSize)
            ? indentSize
            : null;
        this.TabWidth = properties.TryGetValue("tab_width", out var tabWidth) ? tabWidth : null;
        this.MaxLineLength = properties.TryGetValue("max_line_length", out var maxLineLenght)
            ? maxLineLenght
            : null;
    }

    public bool IsMatch(string fileName)
    {
        return this.matcher.IsMatch(fileName);
    }

    private static string FixGlob(string glob, string directory)
    {
        glob = glob.IndexOf('/') switch
        {
            -1 => "**/" + glob,
            0 => glob[1..],
            _ => glob
        };
        directory = directory.Replace(@"\", "/");
        if (!directory.EndsWith("/"))
        {
            directory += "/";
        }

        return directory + glob;
    }
}
