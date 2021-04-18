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
            string file,
            CancellationToken cancellationToken
        ) {
            var defaultedEncoding = false;

            var detectionResult = CharsetDetector.DetectFromFile(file);
            var encoding = detectionResult?.Detected?.Encoding;
            if (encoding == null)
            {
                defaultedEncoding = true;
                encoding = Encoding.Default;
            }

            var fileContents =
                await File.ReadAllTextAsync(
                    file,
                        // this fixes an issue with ANSI encoded files like csharpier-repos\AutoMapper\src\UnitTests\Internationalization.cs
                        encoding.CodePage
                        == 852
                        ? Encoding.GetEncoding(1252)
                        : encoding,
                    cancellationToken
                );

            return new FileReaderResult
            {
                Encoding = encoding,
                DefaultedEncoding = defaultedEncoding,
                FileContents = fileContents
            };
        }
    }

    public class FileReaderResult
    {
        public Encoding Encoding { get; init; }
        public string FileContents { get; init; }
        public bool DefaultedEncoding { get; init; }
    }
}
