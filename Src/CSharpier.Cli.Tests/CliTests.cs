using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Cli.Tests;

// TODO update workflow to run these, or do they run already?

// these tests are kind of nice as c# because they run in the same place.
// they used to be powershell, but doing the multiple file thing didn't work
// that worked in by writing js, but that felt worse than powershell
// these mostly work except the one test that has issues with console input redirection
// the CSharpierProcess abstraction is also a little fragile, but makes for clean tests when they
// are written properly
public class CliTests
{
    private const string TestFileDirectory = "TestFiles";

    [SetUp]
    public void BeforeEachTest()
    {
        if (Directory.Exists(TestFileDirectory))
        {
            Directory.Delete(TestFileDirectory, true);
        }
        Directory.CreateDirectory(TestFileDirectory);
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Basic_File(string lineEnding)
    {
        var formattedContent = "public class ClassName { }" + lineEnding;
        var unformattedContent = $"public class ClassName {{{lineEnding}{lineEnding}}}";

        await WriteFileAsync("BasicFile.cs", unformattedContent);

        var (output, exitCode, _) = await new CsharpierProcess()
            .WithArguments("BasicFile.cs")
            .ExecuteAsync();

        var result = await ReadAllTextAsync("BasicFile.cs");

        output.Should().StartWith("Total time:");
        output
            .Should()
            .Contain(
                "Total files:                                                                           1"
            );
        exitCode.Should().Be(0);
        result.Should().Be(formattedContent);
    }

    [Test]
    public async Task Should_Respect_Ignore_File_With_Subdirectory_When_DirectorOrFile_Is_Dot()
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = "Subdirectory/IgnoredFile.cs";
        await WriteFileAsync(filePath, unformattedContent);
        await WriteFileAsync(".csharpierignore", filePath);

        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var result = await ReadAllTextAsync(filePath);

        result.Should().Be(unformattedContent, $"The file at {filePath} should have been ignored");
    }

    [Test]
    public async Task Should_Return_Error_When_No_DirectoryOrFile_And_Not_Piping_StdIn()
    {
        if (Console.IsInputRedirected)
        {
            // This test cannot run if Console.IsInputRedirected is true.
            // Running it from the command line is required.
            // See https://github.com/dotnet/runtime/issues/1147"
            return;
        }

        var (output, exitCode, errorOutput) = await new CsharpierProcess().ExecuteAsync();

        exitCode.Should().Be(1);
        errorOutput
            .Should()
            .Contain("directoryOrFile is required when not piping stdin to CSharpier");
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Piped_File(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";

        using var process = new CsharpierProcess().WithPipedInput().Start();

        process.Write(unformattedContent1);

        var result = await process.GetOutput();

        result.Should().Be(formattedContent1);
    }

    [Test]
    public async Task With_Check_Should_Write_Unformatted_File()
    {
        var unformattedContent = "public class ClassName1 {\n\n}";

        await WriteFileAsync("CheckUnformatted.cs", unformattedContent);

        var (result, exitCode, _) = await new CsharpierProcess()
            .WithArguments("CheckUnformatted.cs --check")
            .ExecuteAsync();

        result.Should().StartWith("Warning /CheckUnformatted.cs - Was not formatted.");
        exitCode.Should().Be(1);
    }

    // TODO file with compilation error should spit out warning for regular use
    // TODO file with compilation error should return file for piping, multi file and single file

    // TODO test with ignored file?
    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Cli_Should_Format_Multiple_Piped_Files(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var formattedContent2 = "public class ClassName2 { }" + lineEnding;
        ;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";
        var unformattedContent2 = $"public class ClassName2 {{{lineEnding}{lineEnding}}}";

        using var process = new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput()
            .Start();

        process.Write("Test2.cs");
        process.Write('\u0003');
        process.Write(unformattedContent1);
        process.Write('\u0003');

        var result = await process.GetOutputWithoutClosing();

        result.Should().Be(formattedContent1);

        process.Write("Test2.cs");
        process.Write('\u0003');
        process.Write(unformattedContent2);
        process.Write('\u0003');

        result = await process.GetOutputWithoutClosing();

        result.Should().Be(formattedContent2);
    }

    private async Task WriteFileAsync(string path, string content)
    {
        var fileInfo = new FileInfo(Path.Combine(TestFileDirectory, path));
        EnsureExists(fileInfo.Directory!);

        await File.WriteAllTextAsync(fileInfo.FullName, content);
    }

    private async Task<string> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(Path.Combine(TestFileDirectory, path));
    }

    private void EnsureExists(DirectoryInfo directoryInfo)
    {
        if (directoryInfo.Parent != null)
        {
            EnsureExists(directoryInfo.Parent);
        }

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }

    private class CsharpierProcess : IDisposable
    {
        private StreamWriter? standardInput;
        private readonly Process process;
        private bool started;

        public CsharpierProcess()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.exe");

            this.process = new Process();
            this.process.StartInfo.WorkingDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                TestFileDirectory
            );
            this.process.StartInfo.FileName = path;
            this.process.StartInfo.UseShellExecute = false;
            this.process.StartInfo.RedirectStandardInput = false;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
        }

        public CsharpierProcess WithArguments(string arguments)
        {
            this.process.StartInfo.Arguments = arguments;

            return this;
        }

        public CsharpierProcess WithPipedInput()
        {
            this.process.StartInfo.RedirectStandardInput = true;

            return this;
        }

        public CsharpierProcess Start()
        {
            this.started = true;
            this.process.Start();

            if (this.process.StartInfo.RedirectStandardInput)
            {
                this.standardInput = this.process.StandardInput;
            }

            return this;
        }

        public void Write(string value)
        {
            if (!this.process.StartInfo.RedirectStandardInput)
            {
                throw new Exception("WithPipedInput was not called");
            }

            this.standardInput?.Write(value);
            Console.WriteLine("Write: " + value);
        }

        public void Write(char value)
        {
            if (!this.process.StartInfo.RedirectStandardInput)
            {
                throw new Exception("WithPipedInput was not called");
            }

            this.standardInput?.Write(value);
            Console.WriteLine("Write: " + value);
        }

        public Task<string> GetOutputWithoutClosing()
        {
            return this.GetOutput(false);
        }

        public Task<string> GetOutput()
        {
            return this.GetOutput(true);
        }

        public async Task<(string output, int exitCode, string errorOutput)> ExecuteAsync()
        {
            this.started = true;
            this.process.Start();
            await this.process.WaitForExitAsync();
            var output = await this.process.StandardOutput.ReadToEndAsync();
            var errorOutput = await this.process.StandardError.ReadToEndAsync();
            var exitCode = this.process.ExitCode;
            this.Dispose();

            return (output, exitCode, errorOutput);
        }

        private async Task<string> GetOutput(bool close)
        {
            if (!this.started)
            {
                throw new Exception("The process was never started");
            }

            if (close)
            {
                this.standardInput?.Close();
                await this.process.WaitForExitAsync();
                return await this.process.StandardOutput.ReadToEndAsync();
            }

            var result = new StringBuilder();
            while (true)
            {
                result.Append(Convert.ToChar(this.process.StandardOutput.Read()));

                // wait 10ms to see if there will be another character to read
                var x = 0;
                while (this.process.StandardOutput.Peek() < 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                    x++;
                    if (x > 10)
                    {
                        return result.ToString();
                    }
                }
            }
        }

        public void Dispose()
        {
            this.standardInput?.Dispose();
            if (this.started)
            {
                this.process.Kill();
                this.process.Dispose();
            }
        }
    }
}
