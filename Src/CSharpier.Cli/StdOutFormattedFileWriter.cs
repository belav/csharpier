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
        File.AppendAllText("c:/temp/log.txt", "Output:");
        File.AppendAllText("c:/temp/log.txt", result.Code);

        this.console.Write(result.Code);
    }
}
