using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier
{
    public class IgnoreFile
    {
        protected global::Ignore.Ignore Ignore { get; }
        protected string IgnoreBaseDirectoryPath { get; }

        protected IgnoreFile(global::Ignore.Ignore ignore, string ignoreBaseDirectoryPath)
        {
            this.Ignore = ignore;
            this.IgnoreBaseDirectoryPath = ignoreBaseDirectoryPath;
        }

        public bool IsIgnored(string filePath)
        {
            if (!filePath.StartsWith(this.IgnoreBaseDirectoryPath))
            {
                throw new Exception(
                    "The filePath of "
                    + filePath
                    + " does not start with the ignoreBaseDirectoryPath of "
                    + this.IgnoreBaseDirectoryPath
                );
            }

            var normalizedFilePath = filePath.Replace("\\", "/")
                .Substring(this.IgnoreBaseDirectoryPath.Length + 1);

            return this.Ignore.IsIgnored(normalizedFilePath);
        }

        public static async Task<(IgnoreFile?, int)> Create(
            string baseDirectoryPath,
            IFileSystem fileSystem,
            IConsole console,
            CancellationToken cancellationToken
        ) {
            var ignore = new global::Ignore.Ignore();
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
                    return (new IgnoreFile(ignore, baseDirectoryPath), 0);
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
                    return (null, 1);
                }
            }

            return (new IgnoreFile(ignore, directoryInfo.FullName), 0);
        }
    }
}
