using System.IO.Abstractions;

namespace CSharpier.Cli;

internal class IgnoreFile
{
    protected Ignore.Ignore Ignore { get; }
    protected string IgnoreBaseDirectoryPath { get; }
    private static readonly string[] alwaysIgnored = ["**/node_modules", "**/obj", "**/.git"];

    protected IgnoreFile(Ignore.Ignore ignore, string ignoreBaseDirectoryPath)
    {
        this.Ignore = ignore;
        this.IgnoreBaseDirectoryPath = ignoreBaseDirectoryPath.Replace('\\', '/');
    }

    public bool IsIgnored(string filePath)
    {
        var normalizedFilePath = filePath.Replace('\\', '/');
        if (!normalizedFilePath.StartsWith(this.IgnoreBaseDirectoryPath, StringComparison.Ordinal))
        {
            throw new Exception(
                "The filePath of "
                    + filePath
                    + " does not start with the ignoreBaseDirectoryPath of "
                    + this.IgnoreBaseDirectoryPath
            );
        }

        normalizedFilePath =
            normalizedFilePath.Length > this.IgnoreBaseDirectoryPath.Length
                ? normalizedFilePath[(this.IgnoreBaseDirectoryPath.Length + 1)..]
                : string.Empty;

        return this.Ignore.IsIgnored(normalizedFilePath);
    }

    public static async Task<IgnoreFile> Create(
        string baseDirectoryPath,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        var ignore = new Ignore.Ignore();

        foreach (var name in alwaysIgnored)
        {
            ignore.Add(name);
        }

        var ignoreFilePaths = FindIgnorePath(baseDirectoryPath, fileSystem);
        if (ignoreFilePaths.Count == 0)
        {
            return new IgnoreFile(ignore, baseDirectoryPath);
        }

        foreach (var ignoreFilePath in ignoreFilePaths)
        foreach (
            var line in await fileSystem.File.ReadAllLinesAsync(ignoreFilePath, cancellationToken)
        )
        {
            try
            {
                ignore.Add(line);
            }
            catch (Exception ex)
            {
                throw new InvalidIgnoreFileException(
                    @$"The .csharpierignore file at {ignoreFilePath} could not be parsed due to the following line:
{line}
",
                    ex
                );
            }
        }

        // TODO #631 htf is this going to work? each ignorefile needs one, ugh
        var directoryName = fileSystem.Path.GetDirectoryName(ignoreFilePaths.Last());

        ArgumentNullException.ThrowIfNull(directoryName);

        return new IgnoreFile(ignore, directoryName);
    }

    // this will return the ignore paths with the priority path coming last, which means the rules from the last entry will take
    // priority. .csharpierignore comes last, gitignore come before
    private static List<string> FindIgnorePath(string baseDirectoryPath, IFileSystem fileSystem)
    {
        var result = new List<string>();
        string? foundCSharpierIgnoreFilePath = null;
        var directoryInfo = fileSystem.DirectoryInfo.New(baseDirectoryPath);
        while (directoryInfo != null)
        {
            if (foundCSharpierIgnoreFilePath is null)
            {
                var csharpierIgnoreFilePath = fileSystem.Path.Combine(
                    directoryInfo.FullName,
                    ".csharpierignore"
                );
                if (fileSystem.File.Exists(csharpierIgnoreFilePath))
                {
                    foundCSharpierIgnoreFilePath = csharpierIgnoreFilePath;
                }
            }

            var gitIgnoreFilePath = fileSystem.Path.Combine(directoryInfo.FullName, ".gitignore");
            if (fileSystem.File.Exists(gitIgnoreFilePath))
            {
                result.Insert(0, gitIgnoreFilePath);
            }

            directoryInfo = directoryInfo.Parent;
        }

        if (foundCSharpierIgnoreFilePath is not null)
        {
            result.Add(foundCSharpierIgnoreFilePath);
        }

        return result;
    }
}

internal class InvalidIgnoreFileException(string message, Exception exception)
    : Exception(message, exception);
