namespace CSharpier.Benchmarks;

public static class Paths
{
    public static readonly string RepoRoot = GetRepoRoot();

    private static string GetRepoRoot()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (
            currentDirectory != null
            && !Directory.Exists(Path.Combine(currentDirectory.FullName, ".git"))
        )
        {
            currentDirectory = currentDirectory.Parent;
        }

        if (currentDirectory is null)
        {
            throw new Exception("Could not find .git directory");
        }

        return currentDirectory.FullName;
    }
}
