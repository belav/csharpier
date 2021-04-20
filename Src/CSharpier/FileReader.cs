using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtfUnknown;

namespace CSharpier
{
    public static class FileReader
    {
        static FileReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static async Task<FileReaderResult> ReadFile(
            string filePath,
            CancellationToken cancellationToken
        ) {
            var defaultedEncoding = false;

            using FileStream fileStream = File.OpenRead(filePath);
            var detectionResult = CharsetDetector.DetectFromStream(fileStream);
            var encoding = detectionResult?.Detected?.Encoding;
            if (encoding == null)
            {
                defaultedEncoding = true;
                encoding = Encoding.Default;
            }

            fileStream.Seek(0, SeekOrigin.Begin);

            // this fixes an issue with ANSI encoded files like csharpier-repos\AutoMapper\src\UnitTests\Internationalization.cs
            var encodingToRead = encoding.CodePage == 852
                ? Encoding.GetEncoding(1252)
                : encoding;

            using var streamReader = new StreamReader(
                fileStream,
                encodingToRead
            );

            var fileContents = streamReader.ReadToEnd();

            return new FileReaderResult(
                encoding,
                fileContents,
                defaultedEncoding
            );
        }
    }

    public record FileReaderResult(
        Encoding Encoding,
        string FileContents,
        bool DefaultedEncoding
    );
}
