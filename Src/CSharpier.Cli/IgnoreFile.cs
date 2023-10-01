using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

public class IgnoreFile
{
    protected Ignore.Ignore Ignore { get; }
    protected string IgnoreBaseDirectoryPath { get; }
    private static readonly string[] alwaysIgnored =
    {
        "**/node_modules/**/*.cs",
        "**/obj/**/*.cs"
    };

    protected IgnoreFile(Ignore.Ignore ignore, string ignoreBaseDirectoryPath)
    {
        this.Ignore = ignore;
        this.IgnoreBaseDirectoryPath = ignoreBaseDirectoryPath.Replace('\\', '/');
    }

    public bool IsIgnored(string filePath)
    {
        var normalizedFilePath = filePath.Replace('\\', '/');
        if (!normalizedFilePath.StartsWith(this.IgnoreBaseDirectoryPath))
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

        return new IgnoreFile(ignore, fileSystem.Path.GetDirectoryName(ignoreFilePath));
    }

    private static string? FindIgnorePath(string baseDirectoryPath, IFileSystem fileSystem)
    {
        var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(baseDirectoryPath);
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

public class InvalidIgnoreFileException : Exception
{
    public InvalidIgnoreFileException(string message, Exception exception)
        : base(message, exception) { }
}
