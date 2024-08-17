namespace CSharpier.Cli.EditorConfig;

internal static class Globber
{
    private static readonly GlobMatcherOptions globOptions =
        new()
        {
            MatchBase = true,
            Dot = true,
            AllowWindowsPaths = true,
            AllowSingleBraceSets = true,
        };

    public static GlobMatcher Create(string files, string directory)
    {
        var pattern = FixGlob(files, directory);
        return GlobMatcher.Create(pattern, globOptions);
    }

    private static string FixGlob(string glob, string directory)
    {
        glob = glob.IndexOf('/') switch
        {
            -1 => "**/" + glob,
            0 => glob[1..],
            _ => glob,
        };
        directory = directory.Replace(@"\", "/");
        if (!directory.EndsWith("/"))
        {
            directory += "/";
        }

        return directory + glob;
    }
}
