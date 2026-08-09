using CSharpier.Core;

namespace CSharpier.Cli;

internal class StdOutFormattedFileWriter(IConsole console) : IFormattedFileWriter
{
    public bool SupportsParallelWrites => false;

    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo)
    {
        console.Write(result.Code);
    }
}
