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
        var pathRelativeToIgnoreFile = filePath.Replace('\\', '/');
        if (
            !pathRelativeToIgnoreFile.StartsWith(
                this.IgnoreBaseDirectoryPath,
                StringComparison.Ordinal
            )
        )
        {
            // TODO #631
            // the ignore file was created for /test/subfolder
            // and we are checking if /test/.editorconfig is ignored, which it CAN'T be
            // in main, we did not check if parent folder editorconfigs were ignored, which is probably a bug
            // we need to figure out a better way to deal with ignorefiles, which will come in the PR for 631

            return false;
            // throw new Exception(
            //     $"The filePath of {filePath} does not start with the ignoreBaseDirectoryPath of {this.IgnoreBaseDirectoryPath}"
            // );
        }

        pathRelativeToIgnoreFile =
            pathRelativeToIgnoreFile.Length > this.IgnoreBaseDirectoryPath.Length
                ? pathRelativeToIgnoreFile[(this.IgnoreBaseDirectoryPath.Length + 1)..]
                : string.Empty;

        return this.Ignore.IsIgnored(pathRelativeToIgnoreFile);
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

        var ignoreFilePath = FindIgnorePath(baseDirectoryPath, fileSystem);
        if (ignoreFilePath == null)
        {
            return new IgnoreFile(ignore, baseDirectoryPath);
        }

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

        var directoryName = fileSystem.Path.GetDirectoryName(ignoreFilePath);

        ArgumentNullException.ThrowIfNull(directoryName);

        return new IgnoreFile(ignore, directoryName);
    }

    private static string? FindIgnorePath(string baseDirectoryPath, IFileSystem fileSystem)
    {
        var directoryInfo = fileSystem.DirectoryInfo.New(baseDirectoryPath);
        while (directoryInfo != null)
        {
            var ignoreFilePath = fileSystem.Path.Combine(
                directoryInfo.FullName,
                ".csharpierignore"
            );
            if (fileSystem.File.Exists(ignoreFilePath))
            {
                return ignoreFilePath;
            }

            directoryInfo = directoryInfo.Parent;
        }

        return null;
    }
}

internal class InvalidIgnoreFileException(string message, Exception exception)
    : Exception(message, exception);
