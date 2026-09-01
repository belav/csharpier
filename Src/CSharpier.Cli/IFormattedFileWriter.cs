using CSharpier.Core;

namespace CSharpier.Cli;

internal interface IFormattedFileWriter
{
    bool SupportsParallelWrites { get; }
    void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo);
}

internal class NullFormattedFileWriter : IFormattedFileWriter
{
    public bool SupportsParallelWrites => true;

    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo) { }
}
