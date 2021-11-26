using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Cli.Tests;

public class Tests
{
    private const string FormattedContent1 = "public class ClassName1 { }";
    private const string FormattedContent2 = "public class ClassName2 { }";
    private const string UnformattedContent1 = "public class ClassName1  { }";
    private const string UnformattedContent2 = "public class ClassName2  { }";

    [Test]
    public async Task Cli_Should_Format_Piped_File()
    {
        using var process = new CSharpierProcess();

        process.Write(UnformattedContent1);
        var actual = await process.GetOutput();

        actual.Should().Be(FormattedContent1);
    }

    // TODO test with line endings?
    // TODO test with ignored file?
    // TODO why doesn't the final /n show up? is it trimmed by the process junk?
    [Test]
    public async Task Cli_Should_Format_Multiple_Piped_Files()
    {
        using var process = new CSharpierProcess("--pipe-multiple-files");

        process.Write("Test2.cs");
        process.Write('\u0003');
        process.Write(UnformattedContent1);
        process.Write('\u0003');

        var result = await process.GetOutput();

        result.Should().Be(FormattedContent1);

        process.Write("Test2.cs");
        process.Write('\u0003');
        process.Write(UnformattedContent2);
        process.Write('\u0003');

        result = await process.GetOutput();

        result.Should().Be(FormattedContent2);
    }

    private class CSharpierProcess : IDisposable
    {
        private string output = string.Empty;
        private readonly StreamWriter standardInput;
        private readonly Process process;

        public CSharpierProcess(string? arguments = null)
        {
            const string path = "dotnet-csharpier.exe";

            void OutputHandler(object sender, DataReceivedEventArgs e)
            {
                this.output = e.Data;
            }

            this.process = new Process();
            this.process.StartInfo.FileName = path;
            this.process.StartInfo.Arguments = arguments;
            this.process.StartInfo.UseShellExecute = false;
            this.process.StartInfo.RedirectStandardInput = true;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
            this.process.OutputDataReceived += OutputHandler;
            this.process.ErrorDataReceived += OutputHandler;

            this.process.Start();

            this.standardInput = this.process.StandardInput;

            this.process.BeginOutputReadLine();
            this.process.BeginErrorReadLine();
        }

        public void Write(string value)
        {
            this.standardInput.Write(value);
            Console.WriteLine("Write: " + value);
        }

        public void Write(char value)
        {
            this.standardInput.Write(value);
            Console.WriteLine("Write: " + value);
        }

        public async Task<string> GetOutput()
        {
            var x = 0;
            while (this.output == string.Empty)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                x++;
                if (x > 200)
                {
                    throw new Exception("Waiting took too long");
                }
            }

            var result = this.output;
            this.output = string.Empty;

            return result;
        }

        public void Dispose()
        {
            this.standardInput.Dispose();
            this.process.Dispose();
        }
    }
}
