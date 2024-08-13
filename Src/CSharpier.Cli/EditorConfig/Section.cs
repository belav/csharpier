namespace CSharpier.Cli.EditorConfig;

using IniParser.Model;

internal class Section
{
    private static readonly GlobMatcherOptions globOptions =
        new()
        {
            MatchBase = true,
            Dot = true,
            AllowWindowsPaths = true,
            AllowSingleBraceSets = true,
        };

    private readonly GlobMatcher matcher;

    public string? IndentStyle { get; }
    public string? IndentSize { get; }
    public string? TabWidth { get; }
    public string? MaxLineLength { get; }
    public string? EndOfLine { get; }
    public string? NewLineBeforeOpenBrace { get; }
    public string? NewLineBeforeElse { get; }
    public string? NewLineBeforeCatch { get; }
    public string? NewLineBeforeFinally { get; }
    public string? NewLineBeforeMembersInObjectInitializers { get; }
    public string? NewLineBeforeMembersInAnonymousTypes { get; }
    public string? NewLineBetweenQueryExpressionClauses { get; }

    public Section(SectionData section, string directory)
    {
        var pattern = FixGlob(section.SectionName, directory);
        this.matcher = GlobMatcher.Create(pattern, globOptions);
        this.IndentStyle = section.Keys["indent_style"];
        this.IndentSize = section.Keys["indent_size"];
        this.TabWidth = section.Keys["tab_width"];
        this.MaxLineLength = section.Keys["max_line_length"];
        this.EndOfLine = section.Keys["end_of_line"];
        this.NewLineBeforeOpenBrace = section.Keys["csharp_new_line_before_open_brace"];
        this.NewLineBeforeElse = section.Keys["csharp_new_line_before_else"];
        this.NewLineBeforeCatch = section.Keys["csharp_new_line_before_catch"];
        this.NewLineBeforeFinally = section.Keys["csharp_new_line_before_finally"];
        this.NewLineBeforeMembersInObjectInitializers = section.Keys["csharp_new_line_before_members_in_object_initializers"];
        this.NewLineBeforeMembersInAnonymousTypes = section.Keys["csharp_new_line_before_members_in_anonymous_types"];
        this.NewLineBetweenQueryExpressionClauses = section.Keys["csharp_new_line_between_query_expression_clauses"];
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
