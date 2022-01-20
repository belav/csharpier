using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Cli.Tests;

// these tests are kind of nice as c# because they run in the same place.
// except the one test that has issues with console input redirection
// they used to be powershell, but doing the multiple file thing didn't work
// that worked in by writing js, but that felt worse than powershell
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

        await this.WriteFileAsync("BasicFile.cs", unformattedContent);

        var result = await new CsharpierProcess().WithArguments("BasicFile.cs").ExecuteAsync();

        result.Output.Should().StartWith("Total time:");
        result.Output
            .Should()
            .Contain(
                "Total files:                                                                           1"
            );
        result.ExitCode.Should().Be(0);
        (await this.ReadAllTextAsync("BasicFile.cs")).Should().Be(formattedContent);
    }

    [Test]
    public async Task Should_Respect_Ignore_File_With_Subdirectory_When_DirectorOrFile_Is_Dot()
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = "Subdirectory/IgnoredFile.cs";
        await this.WriteFileAsync(filePath, unformattedContent);
        await this.WriteFileAsync(".csharpierignore", filePath);

        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var result = await this.ReadAllTextAsync(filePath);

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

        var result = await new CsharpierProcess()
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Print_NotFound()
    {
        var result = await new CsharpierProcess().WithArguments("/BasicFile.cs").ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ErrorOutput
            .Should()
            .StartWith("There was no file or directory found at /BasicFile.cs");
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Should_Write_To_StdError_For_Piped_Invalid_File()
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess().WithPipedInput(invalidFile).ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ExitCode.Should().Be(1);
        result.ErrorOutput.Should().Contain("Failed to compile so was not formatted");
    }

    [Test]
    public async Task With_Check_Should_Write_Unformatted_File()
    {
        var unformattedContent = "public class ClassName1 {\n\n}";

        await this.WriteFileAsync("CheckUnformatted.cs", unformattedContent);

        var result = await new CsharpierProcess()
            .WithArguments("CheckUnformatted.cs --check")
            .ExecuteAsync();

        result.Output
            .Replace("\\", "/")
            .Should()
            .StartWith("Warning /CheckUnformatted.cs - Was not formatted.");
        result.ExitCode.Should().Be(1);
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Multiple_Piped_Files(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var formattedContent2 = "public class ClassName2 { }" + lineEnding;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";
        var unformattedContent2 = $"public class ClassName2 {{{lineEnding}{lineEnding}}}";

        var input =
            $"Test1.cs{'\u0003'}{unformattedContent1}{'\u0003'}"
            + $"Test2.cs{'\u0003'}{unformattedContent2}{'\u0003'}";

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput(input)
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        var results = result.Output.Split('\u0003');
        results[0].Should().Be(formattedContent1);
        results[1].Should().Be(formattedContent2);
    }

    [Test]
    public async Task Should_Write_Error_With_Multiple_Piped_Files()
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput($"InvalidFile.cs{'\u0003'}{invalidFile}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput
            .Should()
            .Be(
                $"Error /InvalidFile.cs - Failed to compile so was not formatted.{Environment.NewLine}"
            );
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Should_Ignore_Piped_File_With_Multiple_Piped_Files()
    {
        const string ignoredFile = "public class ClassName {     }";
        var fileName = Path.Combine(testFileDirectory, "Ignored.cs");
        await this.WriteFileAsync(".csharpierignore", "Ignored.cs");

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput($"{fileName}{'\u0003'}{ignoredFile}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().BeEmpty();
    }

    [Test]
    public async Task Should_Support_Config_With_Multiple_Piped_Files()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = Path.Combine(testFileDirectory, "TooWide.cs");
        await this.WriteFileAsync(".csharpierrc", "printWidth: 10");

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput($"{fileName}{'\u0003'}{fileContent}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task Should_Not_Fail_On_Empty_File()
    {
        await this.WriteFileAsync("BasicFile.cs", "");

        var result = await new CsharpierProcess().WithArguments(".").ExecuteAsync();

        result.Output.Should().StartWith("Total time:");
        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
    }

    private async Task WriteFileAsync(string path, string content)
    {
        var fileInfo = new FileInfo(Path.Combine(testFileDirectory, path));
        this.EnsureExists(fileInfo.Directory!);

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
            this.EnsureExists(directoryInfo.Parent);
        }

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }

    private class CsharpierProcess
    {
        private readonly StringBuilder output = new();
        private readonly StringBuilder errorOutput = new();
        private Command command;

        public CsharpierProcess()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll");

            this.command = CliWrap.Cli
                .Wrap("dotnet")
                .WithArguments(path)
                .WithWorkingDirectory(testFileDirectory)
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(this.output))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(this.errorOutput));
        }

        public CsharpierProcess WithArguments(string arguments)
        {
            this.command = this.command.WithArguments(this.command.Arguments + " " + arguments);
            return this;
        }

        public CsharpierProcess WithPipedInput(string input)
        {
            this.command = this.command.WithStandardInputPipe(PipeSource.FromString(input));

            return this;
        }

        public async Task<ProcessResult> ExecuteAsync()
        {
            var result = await this.command.ExecuteAsync();
            return new ProcessResult(
                this.output.ToString(),
                this.errorOutput.ToString(),
                result.ExitCode
            );
        }

        public record ProcessResult(string Output, string ErrorOutput, int ExitCode);
    }
}
