using System.Text;

namespace CSharpier.Cli;

internal class StandardInFileInfo : IFileInfo
{
    public string FileContents { get; }
    public Encoding Encoding { get; }
    public bool UnableToDetectEncoding => false;

    public StandardInFileInfo(string standardInFileContents, Encoding consoleInputEncoding)
    {
        this.FileContents = standardInFileContents;
        this.Encoding = consoleInputEncoding;
    }
}
