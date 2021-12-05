using System.IO.Abstractions;
using System.Text;

namespace CSharpier.Cli;

internal class PhysicalFileInfoAndWriter : IFileInfo, IFormattedFileWriter
{
    public string Path { get; }
    public IFileSystem FileSystem { get; }
    public string FileContents { get; }
    public Encoding Encoding { get; }
    public bool UnableToDetectEncoding { get; }

    private PhysicalFileInfoAndWriter(
        string path,
        IFileSystem fileSystem,
        string fileContents,
        Encoding encoding,
        bool unableToDetectEncoding
    )
    {
        this.Path = path;
        this.FileSystem = fileSystem;
        this.FileContents = fileContents;
        this.Encoding = encoding;
        this.UnableToDetectEncoding = unableToDetectEncoding;
    }

    public static async Task<PhysicalFileInfoAndWriter> Create(
        string path,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (encoding, fileContents, unableToDetectEncoding) = await FileReader.ReadFile(
            path,
            fileSystem,
            cancellationToken
        );

        return new PhysicalFileInfoAndWriter(
            path,
            fileSystem,
            fileContents,
            encoding,
            unableToDetectEncoding
        );
    }

    public void WriteResult(CodeFormatterResult result)
    {
        if (result.Code != this.FileContents)
        {
            this.FileSystem.File.WriteAllText(this.Path, result.Code, this.Encoding);
        }
    }
}
