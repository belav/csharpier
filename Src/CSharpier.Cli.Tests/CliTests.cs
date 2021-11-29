using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Cli.Tests;

// these tests are kind of nice as c# because they run in the same place.
// they used to be powershell, but doing the multiple file thing didn't work
// that worked in by writing js, but that felt worse than powershell
// these mostly work except the one test that has issues with console input redirection
// the CSharpierProcess abstraction is also a little fragile, but makes for clean tests when they
// are written properly
public class CliTests
{
    private static readonly string testFileDirectory = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TestFiles"
    );

    [SetUp]
    public void BeforeEachTest()
    {
        if (Directory.Exists(testFileDirectory))
        {
            Directory.Delete(testFileDirectory, true);
        }
        Directory.CreateDirectory(testFileDirectory);
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Basic_File(string lineEnding)
    {
        var formattedContent = "public class ClassName { }" + lineEnding;
        var unformattedContent = $"public class ClassName {{{lineEnding}{lineEnding}}}";

        await WriteFileAsync("BasicFile.cs", unformattedContent);

        var result = await new CsharpierProcess().WithArguments("BasicFile.cs").ExecuteAsync();

        result.Output.Should().StartWith("Total time:");
        result.Output
            .Should()
            .Contain(
                "Total files:                                                                           1"
            );
        result.ExitCode.Should().Be(0);
        (await ReadAllTextAsync("BasicFile.cs")).Should().Be(formattedContent);
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

        var result = await new CsharpierProcess().ExecuteAsync();

        result.ExitCode.Should().Be(1);
        result.ErrorOutput
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

        var result = await process.GetOutputAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Write_To_StdError_For_Piped_Invalid_File()
    {
        const string invalidFile = "public class ClassName { ";

        using var process = new CsharpierProcess().WithPipedInput().Start();

        process.Write(invalidFile);

        var result = await process.GetOutputAsync();

        result.Output.Should().BeEmpty();
        result.ExitCode.Should().Be(1);
        result.ErrorOutput.Should().Contain("Failed to compile so was not formatted");
    }

    [Test]
    public async Task With_Check_Should_Write_Unformatted_File()
    {
        var unformattedContent = "public class ClassName1 {\n\n}";

        await WriteFileAsync("CheckUnformatted.cs", unformattedContent);

        var result = await new CsharpierProcess()
            .WithArguments("CheckUnformatted.cs --check")
            .ExecuteAsync();

        result.Output.Should().StartWith("Warning /CheckUnformatted.cs - Was not formatted.");
        result.ExitCode.Should().Be(1);
    }

    // TODO test with ignored file?
    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Multiple_Piped_Files(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var formattedContent2 = "public class ClassName2 { }" + lineEnding;
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

        process.Write("Test2.cs");
        process.Write('\u0003');
        process.Write(unformattedContent2);
        process.Write('\u0003');

        var result = await process.GetOutputAsync();

        var results = result.Output.Split('\u0003');
        results[0].Should().Be(formattedContent1);
        results[1].Should().Be(formattedContent2);
    }

    [Test]
    public async Task Should_Write_Error_With_Multiple_Piped_Files()
    {
        const string invalidFile = "public class ClassName { ";

        using var process = new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput()
            .Start();

        process.Write("InvalidFile.cs");
        process.Write('\u0003');
        process.Write(invalidFile);
        process.Write('\u0003');

        var result = await process.GetOutputAsync();

        // TODO this should contain the file name
        result.ErrorOutput
            .Should()
            .Be($"Error  - Failed to compile so was not formatted.{Environment.NewLine}");
        result.ExitCode.Should().Be(1);
    }

    private async Task WriteFileAsync(string path, string content)
    {
        var fileInfo = new FileInfo(Path.Combine(testFileDirectory, path));
        EnsureExists(fileInfo.Directory!);

        await File.WriteAllTextAsync(fileInfo.FullName, content);
    }

    private async Task<string> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(Path.Combine(testFileDirectory, path));
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

    // TODO with how much we simplified this, why not just use cliwrap??
    private class CsharpierProcess : IDisposable
    {
        private StreamWriter? standardInput;
        private readonly Process process;
        private bool started;

        public CsharpierProcess()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll");

            this.process = new Process();
            this.process.StartInfo.WorkingDirectory = testFileDirectory;
            this.process.StartInfo.FileName = "dotnet";
            this.process.StartInfo.Arguments = path;
            this.process.StartInfo.UseShellExecute = false;
            this.process.StartInfo.RedirectStandardInput = false;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
        }

        public CsharpierProcess WithArguments(string arguments)
        {
            this.process.StartInfo.Arguments += " " + arguments;

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

        public Task<ProcessResult> ExecuteAsync()
        {
            this.started = true;
            this.process.Start();

            return this.GetResult();
        }

        public Task<ProcessResult> GetOutputAsync()
        {
            if (!this.started)
            {
                throw new Exception("The process was never started");
            }

            this.standardInput?.Close();

            return this.GetResult();
        }

        private async Task<ProcessResult> GetResult()
        {
            await this.process.WaitForExitAsync();
            var output = await this.process.StandardOutput.ReadToEndAsync();
            var errorOutput = await this.process.StandardError.ReadToEndAsync();
            var exitCode = this.process.ExitCode;

            return new ProcessResult(output, errorOutput, exitCode);
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

        public record ProcessResult(string Output, string ErrorOutput, int ExitCode);
    }
}
