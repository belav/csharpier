namespace CSharpier.Cli;

internal interface IFormattedFileWriter
{
    void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo);
}

internal class NullFormattedFileWriter : IFormattedFileWriter
{
    public void WriteResult(CodeFormatterResult result, FileToFormatInfo fileToFormatInfo) { }
}
