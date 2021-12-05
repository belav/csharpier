using System.Text;

namespace CSharpier.Cli;

interface IFileInfo
{
    string FileContents { get; }
    Encoding Encoding { get; }
    bool UnableToDetectEncoding { get; }
}
