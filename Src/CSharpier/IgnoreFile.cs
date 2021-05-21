using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier
{
    public class IgnoreFile
    {
        protected Ignore.Ignore Ignore { get; }
        protected string IgnoreBaseDirectoryPath { get; }

        protected IgnoreFile(Ignore.Ignore ignore, string ignoreBaseDirectoryPath)
        {
            this.Ignore = ignore;
            this.IgnoreBaseDirectoryPath = ignoreBaseDirectoryPath.Replace("\\", "/");
        }

        public bool IsIgnored(string filePath)
        {
            var normalizedFilePath = filePath.Replace("\\", "/");
            if (!normalizedFilePath.StartsWith(this.IgnoreBaseDirectoryPath))
            {
                throw new Exception(
                    "The filePath of "
                    + filePath
                    + " does not start with the ignoreBaseDirectoryPath of "
                    + this.IgnoreBaseDirectoryPath
                );
            }

            normalizedFilePath = normalizedFilePath.Substring(
                this.IgnoreBaseDirectoryPath.Length + 1
            );

            return this.Ignore.IsIgnored(normalizedFilePath);
        }

        public static async Task<IgnoreFile?> Create(
            string baseDirectoryPath,
            IFileSystem fileSystem,
            IConsole console,
            CancellationToken cancellationToken
        ) {
            var ignore = new Ignore.Ignore();
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(baseDirectoryPath);
            var ignoreFilePath = fileSystem.Path.Combine(
                directoryInfo.FullName,
                ".csharpierignore"
            );
            while (!fileSystem.File.Exists(ignoreFilePath))
            {
                directoryInfo = directoryInfo.Parent;
                if (directoryInfo == null)
                {
                    return new IgnoreFile(ignore, baseDirectoryPath);
                }
                ignoreFilePath = fileSystem.Path.Combine(
                    directoryInfo.FullName,
                    ".csharpierignore"
                );
            }

            foreach (
                var line in await fileSystem.File.ReadAllLinesAsync(
                    ignoreFilePath,
                    cancellationToken
                )
            ) {
                try
                {
                    ignore.Add(line);
                }
                catch (Exception ex)
                {
                    console.WriteLine(
                        "The .csharpierignore file at "
                        + ignoreFilePath
                        + " could not be parsed due to the following line:"
                    );
                    console.WriteLine(line);
                    console.WriteLine("Exception: " + ex.Message);
                    return null;
                }
            }

            return new IgnoreFile(ignore, directoryInfo.FullName);
        }
    }
}
