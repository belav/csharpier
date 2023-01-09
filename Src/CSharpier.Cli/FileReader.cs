using System.IO.Abstractions;
using System.Text;

namespace CSharpier.Cli;

internal static class FileReader
{
    private static readonly SemaphoreSlim Semaphore = new(10);

    static FileReader()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static async Task<FileReaderResult> ReadFile(
        string filePath,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        var unableToDetectEncoding = false;

        await Semaphore.WaitAsync(cancellationToken);
        try
        {
            await using var fileStream = fileSystem.File.OpenRead(filePath);

            using var streamReader = new StreamReader(fileStream, new UTF8Encoding(false));

            var fileContents = await streamReader.ReadToEndAsync();

            return new FileReaderResult(
                streamReader.CurrentEncoding,
                fileContents,
                unableToDetectEncoding
            );
        }
        finally
        {
            Semaphore.Release();
        }
    }
}

internal record FileReaderResult(
    Encoding Encoding,
    string FileContents,
    bool UnableToDetectEncoding
);
