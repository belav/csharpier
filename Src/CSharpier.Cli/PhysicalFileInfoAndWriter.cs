using System.IO.Abstractions;

namespace CSharpier.Cli;

internal class FileSystemFormattedFileWriter : IFormattedFileWriter
{
    public IFileSystem FileSystem { get; }

    public FileSystemFormattedFileWriter(IFileSystem fileSystem)
    {
        this.FileSystem = fileSystem;
    }

    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo)
    {
        if (result.Code != fileToFormatInfo.FileContents)
        {
            this.FileSystem
                .File
                .WriteAllText(fileToFormatInfo.Path, result.Code, fileToFormatInfo.Encoding);
        }
    }
}
