namespace CSharpier.Cli;

internal class StdOutFormattedFileWriter(IConsole console) : IFormattedFileWriter
{
    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo)
    {
        console.Write(result.Code);
    }
}
