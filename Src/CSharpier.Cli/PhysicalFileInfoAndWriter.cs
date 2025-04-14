using System.IO.Abstractions;
using CSharpier.Core;

namespace CSharpier.Cli;

internal class FileSystemFormattedFileWriter(IFileSystem fileSystem) : IFormattedFileWriter
{
    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo)
    {
        if (result.Code != fileToFormatInfo.FileContents)
        {
            fileSystem.File.WriteAllText(
                fileToFormatInfo.Path,
                result.Code,
                fileToFormatInfo.Encoding
            );
        }
    }
}
