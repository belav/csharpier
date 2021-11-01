using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtfUnknown;

namespace CSharpier
{
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
                var detectionResult = CharsetDetector.DetectFromStream(fileStream);
                var encoding = detectionResult?.Detected?.Encoding;
                if (encoding == null)
                {
                    unableToDetectEncoding = true;
                    encoding = Encoding.Default;
                }

                fileStream.Seek(0, SeekOrigin.Begin);

                // this fixes an issue with ANSI encoded files like csharpier-repos\AutoMapper\src\UnitTests\Internationalization.cs
                var encodingToRead =
                    encoding.CodePage == 852 ? Encoding.GetEncoding(1252) : encoding;

                using var streamReader = new StreamReader(fileStream, encodingToRead);

                var fileContents = await streamReader.ReadToEndAsync();

                return new FileReaderResult(encoding, fileContents, unableToDetectEncoding);
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
}
