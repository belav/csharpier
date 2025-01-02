using System.IO.Abstractions;

namespace CSharpier.Cli;

internal class FileSystemFormattedFileWriter(IFileSystem fileSystem) : IFormattedFileWriter
{
    public IFileSystem FileSystem { get; } = fileSystem;

    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo)
    {
        if (result.Code != fileToFormatInfo.FileContents)
        {
            const int maxTries = 5;
            var tries = 1;
            // TODO #819 this started happening on the EFcore folder, is it related to the msbuild check somehow?
            while (tries <= maxTries)
            {
                try
                {
                    this.FileSystem.File.WriteAllText(
                        fileToFormatInfo.Path,
                        result.Code,
                        fileToFormatInfo.Encoding
                    );
                    return;
                }
                catch (IOException)
                {
                    if (tries == maxTries)
                    {
                        throw;
                    }

                    Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    tries++;
                }
            }
        }
    }
}
