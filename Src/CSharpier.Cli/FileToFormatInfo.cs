using System.IO.Abstractions;
using System.Text;

namespace CSharpier.Cli;

internal class FileToFormatInfo
{
    protected FileToFormatInfo() { }

    public string FileContents { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public Encoding Encoding { get; init; } = Encoding.Default;

    public static async Task<FileToFormatInfo> CreateFromFileSystem(
        string path,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (encoding, fileContents) = await FileReader.ReadFileAsync(
            path,
            fileSystem,
            cancellationToken
        );

        return new FileToFormatInfo
        {
            Path = path,
            FileContents = fileContents,
            Encoding = encoding,
        };
    }

    public static FileToFormatInfo Create(string path, string fileContents, Encoding encoding)
    {
        return new FileToFormatInfo
        {
            Path = path,
            FileContents = fileContents,
            Encoding = encoding,
        };
    }
}
