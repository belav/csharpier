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

    public static async Task<FileReaderResult> ReadFileAsync(
        string filePath,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        await Semaphore.WaitAsync(cancellationToken);
        try
        {
            await using var fileStream = fileSystem.File.OpenRead(filePath);

            // this is kind of a "hack" - read the file without the BOM then
            // streamReader.CurrentEncoding below will correctly be UTF8 or UTF8-BOM
            // https://stackoverflow.com/a/27976558
            using var streamReader = new StreamReader(
                fileStream,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
            );

            var fileContents = await streamReader.ReadToEndAsync(cancellationToken);

            var hasInvalidBytes = false;
            if (streamReader.CurrentEncoding is UTF8Encoding && fileContents.Contains('�'))
            {
                hasInvalidBytes = !IsValidUtf8(
                    await fileSystem.File.ReadAllBytesAsync(filePath, cancellationToken)
                );
            }

            return new FileReaderResult(streamReader.CurrentEncoding, fileContents)
            {
                HasInvalidBytes = hasInvalidBytes,
            };
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private static bool IsValidUtf8(byte[] bytes)
    {
        try
        {
            new UTF8Encoding(false, throwOnInvalidBytes: true).GetCharCount(bytes);
            return true;
        }
        catch (DecoderFallbackException)
        {
            return false;
        }
    }
}

internal record FileReaderResult(Encoding Encoding, string FileContents)
{
    public bool HasInvalidBytes { get; init; }
}
