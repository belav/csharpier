namespace CSharpier.Cli;

internal class StdOutFormattedFileWriter : IFormattedFileWriter
{
    private readonly IConsole console;

    public StdOutFormattedFileWriter(IConsole console)
    {
        this.console = console;
    }

    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo)
    {
        this.console.Write(result.Code);
    }
}
